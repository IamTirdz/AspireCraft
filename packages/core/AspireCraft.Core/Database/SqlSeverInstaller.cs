using AspireCraft.Core.Common.Abstractions;
using AspireCraft.Core.Common.Enums;
using AspireCraft.Core.Common.Models;
using AspireCraft.Core.Renderers;

namespace AspireCraft.Core.Database;

public sealed class SqlSeverInstaller : IPackageInstaller
{
    public bool CanInstall(ProjectConfiguration configuration)
    {
        return configuration.Integrations.Contains(IntegrationType.SqlServer);
    }

    public void Install(TemplateContext context)
    {
        var template = Path.Combine("templates", "CleanArchitecture", "Infrastructure", "Persistence", "DbContext.cs.txt");
        var output = Path.Combine("src", "Infrastructure", "Persistence", "DbContext.cs");

        context.Render(template, output);
        context.AddPackage("Microsoft.EntityFrameworkCore.SqlServer");
    }
}
