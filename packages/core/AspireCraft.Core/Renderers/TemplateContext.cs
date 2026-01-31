using AspireCraft.Core.Common.Abstractions;
using AspireCraft.Core.Common.Enums;
using AspireCraft.Core.Common.Extensions;
using AspireCraft.Core.Common.Models;
using System.Diagnostics;

namespace AspireCraft.Core.Renderers;

public sealed class TemplateContext
{
    public ProjectConfiguration Configuration { get; }
    public string ProjectName { get; }
    public string TargetDirectory { get; }
    public ArchitectureType Architecture { get; }
    public NetFramework Framework { get; }
    public IReadOnlyCollection<IntegrationType> Integrations { get; }
    public IDictionary<string, string> Tokens { get; }
    public IProjectRenderer Renderer { get; }

    public TemplateContext(ProjectConfiguration configuration, IProjectRenderer renderer)
    {
        Configuration = configuration;
        ProjectName = configuration.ProjectName;
        Architecture = configuration.Architecture;
        TargetDirectory = Path.Combine(Directory.GetCurrentDirectory(), ProjectName);
        Integrations = new HashSet<IntegrationType>(configuration.Integrations ?? []);

        Tokens = new Dictionary<string, string>
        {
            ["ProjectName"] = ProjectName,
            ["RootNamespace"] = ProjectName,
            ["Architecture"] = Architecture.ToString(),
            ["Framework"] = Framework.ToTargetFramework()
        };

        Renderer = renderer;
    }

    public void Render(string templatePath, string outputPath)
    {
        var fullPath = Path.IsPathRooted(templatePath)
            ? templatePath
            : Path.Combine(AppContext.BaseDirectory, "templates", templatePath);
        fullPath = Path.GetFullPath(fullPath);

        if (!File.Exists(fullPath))
            throw new FileNotFoundException($"Template not found: {fullPath}");

        var outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        var content = File.ReadAllText(fullPath);

        foreach (var token in Tokens)
            content = content.Replace($"{{{{{token.Key}}}}}", token.Value);

        File.WriteAllText(outputPath, content);
    }

    public void AddPackage(string packageName, string projectPath, string? version = null)
    {
        if (string.IsNullOrWhiteSpace(projectPath))
            throw new ArgumentNullException(nameof(projectPath), "Project path cannot be null or empty");

        projectPath = projectPath.Replace('/', Path.DirectorySeparatorChar)
            .Replace('\\', Path.DirectorySeparatorChar)
            .TrimEnd(Path.DirectorySeparatorChar);

        var projectDir = Path.Combine(TargetDirectory, projectPath);
        var projectFile = Path.Combine(projectDir, $"{Path.GetFileName(projectPath)}.csproj");
        if (!File.Exists(projectFile))
            throw new FileNotFoundException($"Project file not found: {projectFile}");

        var versionSuffix = string.IsNullOrEmpty(version) ? "" : $" -v {version}";

        RunDotNet($"add \"{projectFile}\" package {packageName}{versionSuffix}", projectDir);
    }

    public void CreateDirectory()
    {
        Directory.CreateDirectory(TargetDirectory);
    }

    public void RunDotNet(string args, string? workingDir = null)
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = args,
                WorkingDirectory = workingDir ?? TargetDirectory,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false
            }
        };

        process.Start();
        var error = process.StandardError.ReadToEnd();
        process.WaitForExit();

        if (process.ExitCode != 0)
        {
            throw new InvalidOperationException($"Error: {error}");
        }
    }
}
