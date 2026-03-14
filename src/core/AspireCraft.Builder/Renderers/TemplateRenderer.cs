using Scriban;

namespace AspireCraft.Builder.Renderers;

public class TemplateRenderer
{
    public string Render(string templatePath, object model)
    {
        if (!File.Exists(templatePath))
        {
            throw new FileNotFoundException($"Template not found: {templatePath}");
        }

        var content = File.ReadAllText(templatePath);

        var template = Template.Parse(content);
        if (template.HasErrors)
        {
            var errors = string.Join("\n", template.Messages);
            throw new Exception($"Template parsing error:\n{errors}")!;
        }

        return template.Render(model);
    }
}
