using AspireCraft.Core.Common.Constants;

namespace AspireCraft.Core.Renderers;

public sealed class TemplateContext
{
    public string RootPath { get; set; } = string.Empty;

    public void Render(string templatePath, string outputPath)
    {
        var content = File.ReadAllText(templatePath);
        var outputDir = Path.Combine(RootPath, outputPath);

        Directory.CreateDirectory(Path.GetDirectoryName(outputDir)!);
        File.WriteAllText(outputDir, content.Replace(AppConstant.ProjectName, Path.GetFileName(RootPath)));
    }

    public void AddPackage(string packageName)
    {
        // TODO: Integrate with dotnet CLI
        Console.WriteLine($"[NuGet] Would install: {packageName}");
    }
}
