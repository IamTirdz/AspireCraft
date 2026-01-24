using AspireCraft.CLI.Common.Abstractions;
using AspireCraft.CLI.Common.Models;
using AspireCraft.CLI.Renderers;
using AspireCraft.Core.Database.SqlServer;

namespace AspireCraft.Core.Base;

public class CleanArchitecture : ITemplateArchitecture
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

    public void Generate(ProjectConfiguration configuration, GenerationContext context)
    {
        Directory.CreateDirectory(Path.Combine(context.RootPath, "src", "Domain"));
        Directory.CreateDirectory(Path.Combine(context.RootPath, "src", "Application"));
        Directory.CreateDirectory(Path.Combine(context.RootPath, "src", "Infrastructure"));
        Directory.CreateDirectory(Path.Combine(context.RootPath, "src", "Api"));

        foreach (var package in _packages)
        {
            if (package.CanInstall(configuration))
                package.Install(context);
        }
    }
}
