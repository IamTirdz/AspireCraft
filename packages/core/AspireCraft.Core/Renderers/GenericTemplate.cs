using AspireCraft.Core.Common.Abstractions;
using AspireCraft.Core.Common.Enums;
using AspireCraft.Core.Common.Models;

namespace AspireCraft.Core.Renderers;

public class GenericTemplate : IProjectTemplate
{
    public ArchitectureType Architecture { get; }

    private readonly List<IPackageInstaller> _installers;

    public GenericTemplate(IProjectRenderer renderer, IEnumerable<IPackageInstaller>? installers = null)
    {
        Architecture = renderer.Architecture;
        _installers = installers?.ToList() ?? new List<IPackageInstaller>();
    }

    public void Generate(ProjectConfiguration configuration, TemplateContext context)
    {
        context.CreateDirectory();

        context.Renderer.RenderProjects(context);

        foreach (var installer in _installers.Where(p => p.CanInstall(configuration)))
        {
            installer.Install(configuration, context);
        }
    }
}