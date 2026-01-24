using AspireCraft.Core.Common.Models;
using AspireCraft.Core.Renderers;

namespace AspireCraft.Core.Common.Abstractions;

public interface ITemplateArchitecture
{
    string Name { get; }
    void Generate(ProjectConfiguration configuration, TemplateContext context);
}