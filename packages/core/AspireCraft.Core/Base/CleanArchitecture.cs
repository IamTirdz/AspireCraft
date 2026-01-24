using AspireCraft.Core.Common.Abstractions;
using AspireCraft.Core.Common.Enums;
using AspireCraft.Core.Common.Models;
using AspireCraft.Core.Database;
using AspireCraft.Core.Renderers;

namespace AspireCraft.Core.Base;

public sealed class CleanArchitecture : ITemplateArchitecture
{
    public string Name => "CleanArchitecture";
    private readonly List<IPackageInstaller> _packages = [];

    public CleanArchitecture()
    {
        _packages = new List<IPackageInstaller>
        {
            new SqlServerInstaller(),
            new PostgreSqlInstaller(),
            new MySqlInstaller(),
            new SqliteInstaller(),
            new MongoDbInstaller(),
        };
    }

    public void Generate(ProjectConfiguration project, TemplateContext context)
    {
        Directory.CreateDirectory(Path.Combine(context.RootPath, "src", "Domain"));
        Directory.CreateDirectory(Path.Combine(context.RootPath, "src", project.Architecture == ArchitectureType.NLayer ? "Business" : "Application"));
        Directory.CreateDirectory(Path.Combine(context.RootPath, "src", project.Architecture == ArchitectureType.NLayer ? "Data" : "Infrastructure"));
        Directory.CreateDirectory(Path.Combine(context.RootPath, "src", project.Architecture == ArchitectureType.Serverless ? "Function" : "Api"));

        foreach (var package in _packages.Where(p => p.CanInstall(project)))
        {
            package.Install(project, context);
        }
    }
}
