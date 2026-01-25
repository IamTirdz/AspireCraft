using AspireCraft.Core.Common.Enums;
using AspireCraft.Core.Renderers;

namespace AspireCraft.Core.Common.Abstractions;

public interface IProjectRenderer
{
    ArchitectureType Architecture { get; }

    /// <summary>
    /// Creates projects, references, and solution
    /// </summary>
    void RenderProjects(TemplateContext context);

    /// <summary>
    /// Returns the target folder
    /// </summary>
    string GetFolderPath(string path);
}
