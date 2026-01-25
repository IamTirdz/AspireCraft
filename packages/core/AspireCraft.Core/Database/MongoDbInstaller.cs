using AspireCraft.Core.Common.Abstractions;
using AspireCraft.Core.Common.Constants;
using AspireCraft.Core.Common.Enums;
using AspireCraft.Core.Common.Models;
using AspireCraft.Core.Renderers;

namespace AspireCraft.Core.Database;

public sealed class MongoDbInstaller : IPackageInstaller
{
    public bool CanInstall(ProjectConfiguration configuration)
    {
        return configuration.Integrations.Contains(IntegrationType.MongoDB);
    }

    public void Install(ProjectConfiguration configuration, TemplateContext context)
    {
        var directory = context.Renderer.GetFolderPath(AppLayerConstant.DbContext);
        var template = Path.Combine("templates", $"{configuration.Architecture}", directory, "DbContext.cs.txt");
        var output = Path.Combine(context.TargetDirectory, "src", directory, "DbContext.cs");

        context.Render(template, output);
        context.AddPackage("MongoDB.Driver");
    }
}
