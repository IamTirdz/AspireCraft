using AspireCraft.CLI.Common.Constants;
using AspireCraft.CLI.Common.Models;
using Spectre.Console;
using System.Diagnostics;

namespace AspireCraft.CLI.Renderers;

public sealed class ProjectTemplateRenderer
{
    private const string CleanArchitectureTemplate = "clean-architecture";

    public void Generate(ProjectConfiguration config)
    {
        var templateDir = ResolveTemplate(CleanArchitectureTemplate);
        if (!Directory.Exists(templateDir))
        {
            throw new DirectoryNotFoundException($"Template folder not found: {templateDir}");
        }

        var targetDir = Path.Combine(Directory.GetCurrentDirectory(), config.Name);
        if (Directory.Exists(targetDir))
        {
            bool overwrite = AnsiConsole.Confirm("Target folder exists. Overwrite?", false);
            if (!overwrite)
                return;

            Directory.Delete(targetDir, recursive: true);
        }

        AnsiConsole.MarkupLine($"[green]Copying directory...[/]");

        CopyDirectory(templateDir, targetDir);

        //AnsiConsole.MarkupLine($"[green]Replacing token...[/]");
        //ReplaceTokens(targetDir, config.Name);

        AnsiConsole.MarkupLine($"[green]Creating solution...[/]");
        CreateSolution(config.Name, targetDir);

        AnsiConsole.MarkupLine($"[green]Generated backend...[/]");
        ReplaceTokens(targetDir, config.Name);
        AddBackendReferences(config.Name, targetDir);

        AnsiConsole.MarkupLine($"[green]Generated dashboard...[/]");
        AddDashboardReferences(config.Name, targetDir);

        AnsiConsole.MarkupLine($"[green]Generated tests...[/]");
        AddProjectTestReferences(config.Name, targetDir);

        RunDotNet(targetDir, "restore");

        AnsiConsole.MarkupLine($"[green]✔ Project '{config.Name}' generated successfully[/]");
    }

    private static string ResolveTemplate(string templateName)
    {
        var templatesRoot = ResolveTemplatesRoot();
        var templateDir = Path.Combine(templatesRoot, templateName);

        if (!Directory.Exists(templateDir))
            throw new DirectoryNotFoundException($"Template '{templateName}' not found at: {templateDir}");

        return templateDir;
    }

    private static string ResolveTemplatesRoot()
    {
        //var templateDir = Path.Combine(AppContext.BaseDirectory, "templates");
        //if (Directory.Exists(templateDir))
        //    return templateDir;

        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir != null)
        {
            var candidate = Path.Combine(dir.FullName, "templates");
            if (Directory.Exists(candidate))
                return candidate;

            dir = dir.Parent;
        }

        var sourceCandidate = Path.Combine(Directory.GetCurrentDirectory(), "templates");
        if (Directory.Exists(sourceCandidate))
            return sourceCandidate;

        throw new DirectoryNotFoundException("Templates folder not found.");
    }

    private void CreateSolution(string projectName, string targetDir)
    {
        RunDotNet(targetDir, $"new sln -n {projectName}");

        foreach (var csproj in Directory.GetFiles(targetDir, "*.csproj", SearchOption.AllDirectories))
        {
            RunDotNet(targetDir, $"sln add \"{csproj}\"");
        }
    }

    private void AddBackendReferences(string projectName, string targetDir)
    {
        var src = Path.Combine(targetDir, "src");

        var domain = FindProject(src, $"{projectName}.Backend.Domain.csproj");
        var application = FindProject(src, $"{projectName}.Backend.Application.csproj");
        var infrastructure = FindProject(src, $"{projectName}.Backend.Infrastructure.csproj");
        var api = FindProject(src, $"{projectName}.Backend.API.csproj");

        RunDotNet(Path.GetDirectoryName(application)!, $"add reference \"{domain}\"");

        RunDotNet(Path.GetDirectoryName(infrastructure)!, $"add reference \"{application}\"");
        RunDotNet(Path.GetDirectoryName(infrastructure)!, $"add reference \"{domain}\"");

        RunDotNet(Path.GetDirectoryName(api)!, $"add reference \"{application}\"");
        RunDotNet(Path.GetDirectoryName(api)!, $"add reference \"{infrastructure}\"");
    }

    private void AddDashboardReferences(string projectName, string targetDir)
    {
        var dashboard = Path.Combine(targetDir, "dashboard");

        var appHost = FindProject(dashboard, $"{projectName}.AppHost.csproj");
        var serviceDefaults = FindProject(dashboard, $"{projectName}.ServiceDefaults.csproj");
        var api = FindProject(targetDir, $"{projectName}.Backend.API.csproj");

        RunDotNet(Path.GetDirectoryName(appHost)!, $"add reference \"{serviceDefaults}\"");
        RunDotNet(Path.GetDirectoryName(appHost)!, $"add reference \"{api}\"");
        RunDotNet(Path.GetDirectoryName(api)!, $"add reference \"{serviceDefaults}\"");
    }

    private void AddProjectTestReferences(string projectName, string targetDir)
    {
        var test = Path.Combine(targetDir, "tests");

        var unitTests = FindProject(test, $"{projectName}.Backend.UnitTests.csproj");
        var integrationTests = FindProject(test, $"{projectName}.Backend.IntegrationTests.csproj");
        var api = FindProject(targetDir, $"{projectName}.Backend.API.csproj");
        var application = FindProject(targetDir, $"{projectName}.Backend.Application.csproj");

        RunDotNet(Path.GetDirectoryName(unitTests)!, $"add reference \"{application}\"");
        RunDotNet(Path.GetDirectoryName(integrationTests)!, $"add reference \"{api}\"");
    }

    private static void ReplaceTokens(string rootDir, string projectName)
    {
        foreach (var file in Directory.GetFiles(rootDir, "*", SearchOption.AllDirectories))
        {
            var content = File.ReadAllText(file)
                .Replace(AppConstant.ProjectName, projectName);

            File.WriteAllText(file, content);

            var fileName = Path.GetFileName(file);
            if (fileName.Contains(AppConstant.ProjectName))
            {
                var newPath = Path.Combine(Path.GetDirectoryName(file)!, fileName.Replace(AppConstant.ProjectName, projectName));
                if (!File.Exists(newPath))
                    File.Move(file, newPath);
            }
        }

        var directories = Directory
            .GetDirectories(rootDir, "*", SearchOption.AllDirectories)
            .OrderByDescending(d => d.Length)
            .ToList();

        foreach (var dir in directories)
        {
            var name = Path.GetFileName(dir);
            if (!name.Contains(AppConstant.ProjectName))
                continue;

            var newDir = Path.Combine(Path.GetDirectoryName(dir)!, name.Replace(AppConstant.ProjectName, projectName));
            if (!Directory.Exists(newDir))
                Directory.Move(dir, newDir);
        }
    }

    private void CopyDirectory(string source, string target)
    {
        Directory.CreateDirectory(target);

        foreach (var file in Directory.GetFiles(source))
        {
            var destination = Path.Combine(target, Path.GetFileName(file));
            if (File.Exists(destination))
                throw new IOException($"File '{destination}' already exists.");

            File.Copy(file, destination);
        }

        foreach (var dir in Directory.GetDirectories(source))
        {
            CopyDirectory(dir, Path.Combine(target, Path.GetFileName(dir)));
        }
    }

    private void RunDotNet(string workingDir, string args)
    {
        var process = Process.Start(new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = args,
            WorkingDirectory = workingDir,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        });

        process!.WaitForExit();

        if (process.ExitCode != 0)
            throw new InvalidOperationException(process.StandardError.ReadToEnd());
    }

    private static string FindProject(string root, string projectFile)
    {
        return Directory.GetFiles(root, projectFile, SearchOption.AllDirectories).Single();
    }
}
