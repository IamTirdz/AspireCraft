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
        string testDir = Path.Combine(rootDir, "tests");

        var project = new
        {
            Domain = $"{context.ProjectName}.Domain",
            Application = $"{context.ProjectName}.Application",
            Infrastructure = $"{context.ProjectName}.Infrastructure",
            Api = $"{context.ProjectName}.Api"
        };

        CreateSolution(context, rootDir);

        CreateProject(context, srcDir, project.Domain, "classlib");
        CreateProject(context, srcDir, project.Application, "classlib");
        CreateProject(context, srcDir, project.Infrastructure, "classlib");
        CreateProject(context, srcDir, project.Api, "webapi");

        AddNuGetPackage(context, $"{srcDir}/{project.Application}", "AutoMapper.Extensions.Microsoft.DependencyInjection");
        AddNuGetPackage(context, $"{srcDir}/{project.Application}", "FluentValidation");
        AddNuGetPackage(context, $"{srcDir}/{project.Application}", "Ardalis.GuardClauses");
        AddNuGetPackage(context, $"{srcDir}/{project.Application}", "Ardalis.Specification");
        AddNuGetPackage(context, $"{srcDir}/{project.Infrastructure}", "Microsoft.EntityFrameworkCore");
        AddNuGetPackage(context, $"{srcDir}/{project.Infrastructure}", "Microsoft.AspNetCore.Identity.EntityFrameworkCore");
        AddNuGetPackage(context, $"{srcDir}/{project.Infrastructure}", "Serilog");
        AddNuGetPackage(context, $"{srcDir}/{project.Infrastructure}", "Serilog.Extensions.Logging");
        AddNuGetPackage(context, $"{srcDir}/{project.Infrastructure}", "Serilog.Sinks.File");
        AddNuGetPackage(context, $"{srcDir}/{project.Infrastructure}", "Polly");
        AddNuGetPackage(context, $"{srcDir}/{project.Api}", "Swashbuckle.AspNetCore");
        AddNuGetPackage(context, $"{srcDir}/{project.Api}", "Serilog.AspNetCore");
        AddNuGetPackage(context, $"{srcDir}/{project.Api}", "MediatR.Extensions.Microsoft.DependencyInjection");
        AddNuGetPackage(context, $"{srcDir}/{project.Api}", "Microsoft.AspNetCore.ResponseCaching");

        if (context.Configuration.IncludeUnitTests)
        {
            var unitTest = $"{context.ProjectName}.UnitTests";
            CreateProject(context, testDir, unitTest, "xunit");

            AddNuGetPackage(context, $"{testDir}/{unitTest}", "Moq");
            AddNuGetPackage(context, $"{testDir}/{unitTest}", "FluentAssertions");
            AddNuGetPackage(context, $"{testDir}/{unitTest}", "Microsoft.EntityFrameworkCore.InMemory");

            AddReference(context, testDir, unitTest, project.Application);
        }

        if (context.Configuration.IncludeIntegrationTests)
        {
            var integrationTest = $"{context.ProjectName}.IntegrationTests";
            CreateProject(context, testDir, integrationTest, "xunit");

            AddNuGetPackage(context, $"{testDir}/{integrationTest}", "FluentAssertions");
            AddNuGetPackage(context, $"{testDir}/{integrationTest}", "Microsoft.EntityFrameworkCore.InMemory");

            AddReference(context, testDir, integrationTest, project.Api);
        }

        if (context.Configuration.IncludeArchitectureTests)
        {
            var architectureTest = $"{context.ProjectName}.ArchitectureTests";
            CreateProject(context, testDir, architectureTest, "xunit");

            AddNuGetPackage(context, $"{testDir}/{architectureTest}", "FluentAssertions");
            AddNuGetPackage(context, $"{testDir}/{architectureTest}", "NetArchTest.Rules");

            AddReference(context, testDir, architectureTest, project.Domain);
            AddReference(context, testDir, architectureTest, project.Application);
            AddReference(context, testDir, architectureTest, project.Infrastructure);
            AddReference(context, testDir, architectureTest, project.Api);
        }

        AddReference(context, srcDir, project.Application, project.Domain);
        AddReference(context, srcDir, project.Infrastructure, project.Application);
        AddReference(context, srcDir, project.Infrastructure, project.Domain);
        AddReference(context, srcDir, project.Api, project.Application);
        AddReference(context, srcDir, project.Api, project.Infrastructure);

        AddReferencesToSolution(context);
    }

    public string GetFolderPath(string path)
    {
        var directory = path switch
        {
            AppLayerConstant.DbContext => "Persistence",
            AppLayerConstant.Repositories => "Repositories",
            AppLayerConstant.Services => $"Services",
            _ => throw new NotSupportedException($"Layer {path} not defined for Clean Architecture")
        };

        return directory.Replace('/', Path.DirectorySeparatorChar)
            .Replace('\\', Path.DirectorySeparatorChar);
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

    private static void AddNuGetPackage(TemplateContext context, string projectPath, string packageName, string? version = null)
    {
        context.AddPackage(packageName, projectPath, version);
    }
}
