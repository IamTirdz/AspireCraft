using AspireCraft.Core.Common.Abstractions;
using AspireCraft.Core.Common.Constants;
using AspireCraft.Core.Common.Enums;
using AspireCraft.Core.Common.Models;
using AspireCraft.Core.Renderers;

namespace AspireCraft.Core.Database;

public sealed class SqlServerInstaller : IPackageInstaller
{
    public bool CanInstall(ProjectConfiguration configuration)
    {
        return configuration.Integrations.Contains(IntegrationType.SqlServer);
    }

    public void Install(ProjectConfiguration configuration, TemplateContext context)
    {
        var directory = context.Renderer.GetFolderPath(AppLayerConstant.Services);

        var projectDir = Path.Combine(context.TargetDirectory, "src", $"{context.ProjectName}.Infrastructure");
        var contextDir = Path.Combine(projectDir, directory);

        var template = Path.Combine("Integrations", "Database", "SqlServerDbContext.cs.tpl");
        var output = Path.Combine(contextDir, $"{context.Configuration.DbContextName}.cs");

        context.Render(template, output);
        context.AddPackage("Microsoft.EntityFrameworkCore.SqlServer", projectDir);
    }
}
