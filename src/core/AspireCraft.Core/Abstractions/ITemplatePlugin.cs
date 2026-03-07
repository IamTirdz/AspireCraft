using AspireCraft.Core.Models;

namespace AspireCraft.Core.Abstractions;

public interface ITemplatePlugin
{
    string Name { get; }
    TemplateDefinition GetDefinition();
}
