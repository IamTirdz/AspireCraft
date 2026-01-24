using AspireCraft.CLI.Common.Models;
using AspireCraft.CLI.Renderers;

namespace AspireCraft.CLI.Common.Abstractions;

public interface ITemplateArchitecture
{
    string Name { get; }
    void Generate(ProjectConfiguration configuration, GenerationContext context);
}