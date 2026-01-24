using AspireCraft.Core.Common.Abstractions;
using AspireCraft.Core.Common.Models;
using AspireCraft.Core.Database;
using AspireCraft.Core.Renderers;

namespace AspireCraft.Core.Base;

public sealed class CleanArchitecture : ITemplateArchitecture
{
    public string Name => "CleanArchitecture";
    private readonly List<IPackageInstaller> _packages;

    public CleanArchitecture()
    {
        _packages = new List<IPackageInstaller>
        {
            new SqlSeverInstaller(), // TODO
        };
    }

    public void Generate(ProjectConfiguration project, TemplateContext context)
    {
        Directory.CreateDirectory(Path.Combine(context.RootPath, "src", "Domain"));
        Directory.CreateDirectory(Path.Combine(context.RootPath, "src", "Application"));
        Directory.CreateDirectory(Path.Combine(context.RootPath, "src", "Infrastructure"));
        Directory.CreateDirectory(Path.Combine(context.RootPath, "src", "Api"));

        foreach (var package in _packages)
        {
            if (package.CanInstall(project))
                package.Install(context);
        }
    }
}
