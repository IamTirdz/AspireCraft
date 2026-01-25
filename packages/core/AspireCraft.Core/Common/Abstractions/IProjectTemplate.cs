using AspireCraft.Core.Common.Enums;
using AspireCraft.Core.Common.Models;
using AspireCraft.Core.Renderers;

namespace AspireCraft.Core.Common.Abstractions;

public interface IProjectTemplate
{
    ArchitectureType Architecture { get; }
    void Generate(ProjectConfiguration configuration, TemplateContext context);
}
