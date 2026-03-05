using AspireCraft.Builder.Services;
using AspireCraft.Core.Models;
using AspireCraft.Runtime;

namespace AspireCraft.Builder.Generators;

public sealed class ProjectGenerator
{
    private readonly TemplateLoader _loader = new();
    private readonly TemplateRenderer _renderer = new();
    private readonly FileWriter _fileWriter = new();

    public void Generate(ProjectContext context, string outputRoot)
    {
        var templates = _loader.GetAllTemplates();

        foreach (var templateFile in templates)
        {
            var templateContent = File.ReadAllText(templateFile);
            var renderedContent = _renderer.Render(templateContent, context);

            var relativePath = Path.GetRelativePath(_loader.TemplatesRoot, templateFile);
            var outputPath = Path.Combine(outputRoot, relativePath);

            _fileWriter.Write(outputPath, renderedContent);
        }
    }
}
