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
    public IReadOnlySet<IntegrationType> Integrations { get; }
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
        templatePath = templatePath.Replace('/', Path.DirectorySeparatorChar);

        if (!File.Exists(templatePath))
            throw new FileNotFoundException($"Template not found: {templatePath}");

        Directory.CreateDirectory(Path.GetDirectoryName(outputPath)!);
        var content = File.ReadAllText(templatePath);

        foreach (var token in Tokens)
            content = content.Replace($"{{{{{token.Key}}}}}", token.Value);

        File.WriteAllText(outputPath, content);
        Console.WriteLine($"[Render] {outputPath}");
    }

    public void AddPackage(string packageName)
    {
        // TODO
        Console.WriteLine($"[NuGet] Would install: {packageName}");
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
        process.WaitForExit();

        if (process.ExitCode != 0)
            throw new InvalidOperationException(process.StandardError.ReadToEnd());
    }
}
