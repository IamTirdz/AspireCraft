using AspireCraft.Core.Abstractions;
using AspireCraft.Core.Models;
using AspireCraft.Plugins.Templates;

namespace AspireCraft.Builder.Services;

public sealed class ProjectGenerationService
{
    private readonly IEnumerable<ITemplatePlugin> _plugins;

    public ProjectGenerationService()
    {
        _plugins = new ITemplatePlugin[]
        {
            new CleanArchitecturePlugin()
        };
    }

    public TemplateDefinition Generate(ProjectContext context)
    {
        var plugin = _plugins.First(p => p.Name == context.Template);
        return plugin.GetDefinition();
    }
}
