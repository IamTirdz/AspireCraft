using AspireCraft.CLI.Common.Abstractions;
using AspireCraft.CLI.Common.Enums;
using AspireCraft.CLI.Common.Models;
using AspireCraft.CLI.Renderers;

namespace AspireCraft.Core.Database.SqlServer;

public class SqlSeverInstaller : IPackageInstaller
{
    public bool CanInstall(ProjectConfiguration configuration)
    {
        return configuration.Integrations.Contains(IntegrationType.SqlServer);
    }

    public void Install(GenerationContext context)
    {
        var template = Path.Combine("templates", "CleanArchitecture", "Infrastructure", "Persistence", "DbContext.cs.txt");
        var output = Path.Combine("src", "Infrastructure", "Persistence", "DbContext.cs");

        context.Render(template, output);
        context.AddPackage("Microsoft.EntityFrameworkCore.SqlServer");
    }
}
