using AspireCraft.Core.Common.Abstractions;
using AspireCraft.Core.Common.Constants;
using AspireCraft.Core.Common.Enums;
using AspireCraft.Core.Common.Extensions;
using AspireCraft.Core.Renderers;

namespace AspireCraft.Core.Base;

public sealed class CleanArchitectureRenderer : IProjectRenderer
{
    public ArchitectureType Architecture => ArchitectureType.CleanArchitecture;

    public void RenderProjects(TemplateContext context)
    {
        string rootDir = context.TargetDirectory;
        string srcDir = Path.Combine(rootDir, "src");

        var domain = $"{context.ProjectName}.Backend.Domain";
        var application = $"{context.ProjectName}.Backend.Application";
        var infrastructure = $"{context.ProjectName}.Backend.Infrastructure";
        var api = $"{context.ProjectName}.Backend.Api";

        CreateSolution(context, rootDir);

        CreateProject(context, srcDir, domain, "classlib");
        CreateProject(context, srcDir, application, "classlib");
        CreateProject(context, srcDir, infrastructure, "classlib");
        CreateProject(context, srcDir, api, "webapi");

        AddReference(context, srcDir, application, domain);
        AddReference(context, srcDir, infrastructure, application);
        AddReference(context, srcDir, infrastructure, domain);
        AddReference(context, srcDir, api, application);
        AddReference(context, srcDir, api, infrastructure);

        AddReferencesToSolution(context);
    }

    public string GetFolderPath(string path)
    {
        return path switch
        {
            AppLayerConstant.DbContext => "Infrastructure/Persistence",
            AppLayerConstant.Repositories => "Infrastructure/Repositories",
            AppLayerConstant.Services => "Infrastructure/Services",
            _ => throw new NotSupportedException($"Layer {path} not defined for Clean Architecture")
        };
    }

    private static void CreateSolution(TemplateContext context, string rootDir)
    {
        context.RunDotNet($"new sln -n {context.ProjectName}", rootDir);
    }

    private static void CreateProject(TemplateContext context, string rootDir, string projectName, string template)
    {
        Directory.CreateDirectory(rootDir);

        var targetFramework = context.Configuration.Framework.ToTargetFramework();
        context.RunDotNet($"new {template} -n {projectName} -f {targetFramework}", rootDir);
    }

    private static void AddReference(TemplateContext context, string rootDir, string sourceDir, string targetDir)
    {
        var sourceProject = Directory.GetFiles(rootDir, $"{sourceDir}.csproj", SearchOption.AllDirectories).FirstOrDefault();
        var targetProject = Directory.GetFiles(rootDir, $"{targetDir}.csproj", SearchOption.AllDirectories).FirstOrDefault();

        if (sourceProject == null)
            throw new FileNotFoundException($"No .csproj found in {sourceProject}");

        if (targetProject == null)
            throw new FileNotFoundException($"No .csproj found in {targetProject}");

        context.RunDotNet($"add reference \"{targetProject}\"", Path.GetDirectoryName(sourceProject));
    }

    private static void AddReferencesToSolution(TemplateContext context)
    {
        foreach (var csproj in Directory.GetFiles(context.TargetDirectory, "*.csproj", SearchOption.AllDirectories))
        {
            context.RunDotNet($"sln add \"{csproj}\"", context.TargetDirectory);
        }

        context.RunDotNet("restore", context.TargetDirectory);
    }
}
