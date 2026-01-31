using AspireCraft.Core.Common.Abstractions;
using AspireCraft.Core.Common.Constants;
using AspireCraft.Core.Common.Enums;
using AspireCraft.Core.Common.Models;
using AspireCraft.Core.Renderers;

namespace AspireCraft.Core.Database;

public sealed class MySqlInstaller : IPackageInstaller
{
    public bool CanInstall(ProjectConfiguration configuration)
    {
        return configuration.Integrations.Contains(IntegrationType.MySQL);
    }

    public void Install(ProjectConfiguration configuration, TemplateContext context)
    {
        var directory = context.Renderer.GetFolderPath(AppLayerConstant.Services);

        var projectDir = Path.Combine(context.TargetDirectory, "src", $"{context.ProjectName}.Infrastructure");
        var contextDir = Path.Combine(projectDir, directory);

        var template = Path.Combine("Integrations", "Database", "MySqlDbContext.cs.tpl");
        var output = Path.Combine(contextDir, $"{context.Configuration.DbContextName}.cs");

        context.Render(template, output);
        context.AddPackage("Pomelo.EntityFrameworkCore.MySql", projectDir);
    }
}
