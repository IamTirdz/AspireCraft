using AspireCraft.Core.Common.Abstractions;
using AspireCraft.Core.Common.Constants;
using AspireCraft.Core.Common.Enums;
using AspireCraft.Core.Common.Extensions;
using AspireCraft.Core.Renderers;

namespace AspireCraft.Core.Base;

public sealed class ServerlessRenderer : IProjectRenderer
{
    public ArchitectureType Architecture => ArchitectureType.Serverless;

    public void RenderProjects(TemplateContext context)
    {
        string rootDir = context.TargetDirectory;
        string srcDir = Path.Combine(rootDir, "src");

        var contracts = $"{context.ProjectName}.Backend.Contracts";
        var service = $"{context.ProjectName}.Backend.Services";
        var data = $"{context.ProjectName}.Backend.Data";
        var function = $"{context.ProjectName}.Backend.Function";

        CreateSolution(context, rootDir);

        CreateProject(context, srcDir, contracts, "classlib");
        CreateProject(context, srcDir, service, "classlib");
        CreateProject(context, srcDir, data, "classlib");
        CreateFunctionProject(context, srcDir, function);

        AddReference(context, srcDir, service, contracts);
        AddReference(context, srcDir, data, contracts);
        AddReference(context, srcDir, function, service);
        AddReference(context, srcDir, function, data);

        AddReferencesToSolution(context);
    }

    public string GetFolderPath(string path)
    {
        return path switch
        {
            AppLayerConstant.DbContext => "Infrastructure/Persistence",
            AppLayerConstant.Repositories => "Infrastructure/Repositories",
            AppLayerConstant.Services => "Infrastructure/Services",
            _ => throw new NotSupportedException($"Layer {path} not defined for Serverless")
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

    private static void CreateFunctionProject(TemplateContext context, string rootDir, string projectName)
    {
        Directory.CreateDirectory(rootDir);

        var targetFramework = context.Configuration.Framework.ToTargetFramework();
        context.RunDotNet($"new func --worker-runtime dotnet-isolated -n {projectName} --framework {targetFramework}", rootDir);
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
