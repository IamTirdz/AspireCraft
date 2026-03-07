using Scriban;

namespace AspireCraft.Builder.Renderers;

public static class TemplateRenderer
{
    public static string Render(string templatePath, object model)
    {
        var content = File.ReadAllText(templatePath);

        var template = Template.Parse(content);

        return template.Render(model);
    }
}
