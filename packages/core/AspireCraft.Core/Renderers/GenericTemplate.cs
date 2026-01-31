using AspireCraft.Core.Common.Abstractions;
using AspireCraft.Core.Common.Enums;
using AspireCraft.Core.Common.Models;
using System.Xml.Linq;

namespace AspireCraft.Core.Renderers;

public class GenericTemplate : IProjectTemplate
{
    public ArchitectureType Architecture { get; }

    private readonly List<IPackageInstaller> _installers;

    public GenericTemplate(IProjectRenderer renderer, IEnumerable<IPackageInstaller> installers)
    {
        Architecture = renderer.Architecture;
        _installers = installers.ToList();
    }

    public void Generate(ProjectConfiguration configuration, TemplateContext context)
    {
        context.CreateDirectory();

        context.Renderer.RenderProjects(context);

        foreach (var installer in _installers.Where(p => p.CanInstall(configuration)))
        {
            installer.Install(configuration, context);
        }

        foreach (var csproj in Directory.GetFiles(context.TargetDirectory, "*.csproj", SearchOption.AllDirectories))
        {
            NormalizeCsproj(csproj);
        }
    }

    private static void NormalizeCsproj(string csprojPath)
    {
        var doc = XDocument.Load(csprojPath);

        var project = doc.Root!;
        var propertyGroups = project.Elements("PropertyGroup").ToList();
        var itemGroups = project.Elements("ItemGroup").ToList();

        var projectRefs = itemGroups
            .SelectMany(g => g.Elements("ProjectReference"))
            .ToList();

        var packageRefs = itemGroups
            .SelectMany(g => g.Elements("PackageReference"))
            .ToList();

        project.RemoveNodes();

        foreach (var pg in propertyGroups)
        {
            project.Add(pg);
        }

        if (projectRefs.Any())
        {
            project.Add(new XElement("ItemGroup", projectRefs));
        }

        if (packageRefs.Any())
        {
            project.Add(new XElement("ItemGroup", packageRefs));
        }

        doc.Save(csprojPath);
    }
}