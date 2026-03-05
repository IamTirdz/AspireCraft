using AspireCraft.Core.Utilities;

namespace AspireCraft.Builder.Services;

public sealed class TemplateLoader
{
    private readonly string _templatesRoot;

    public TemplateLoader()
    {
        _templatesRoot = TemplateHelper.GetTemplatesPath();
    }

    public string Load(string templateName)
    {
        var path = Path.Combine(_templatesRoot, templateName);
        if (!File.Exists(path))
            throw new FileNotFoundException($"Template {templateName} not found at {path}");

        return File.ReadAllText(path);
    }

    public IEnumerable<string> GetAllTemplates()
    {
        return Directory.EnumerateFiles(_templatesRoot, "*.tpl", SearchOption.AllDirectories);
    }

    public string TemplatesRoot => _templatesRoot;
}
