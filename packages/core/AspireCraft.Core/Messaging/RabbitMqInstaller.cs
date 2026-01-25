using AspireCraft.Core.Common.Abstractions;
using AspireCraft.Core.Common.Constants;
using AspireCraft.Core.Common.Enums;
using AspireCraft.Core.Common.Models;
using AspireCraft.Core.Renderers;

namespace AspireCraft.Core.Messaging;

public sealed class RabbitMqInstaller : IPackageInstaller
{
    public bool CanInstall(ProjectConfiguration configuration)
    {
        return configuration.Integrations.Contains(IntegrationType.RabbitMQ);
    }

    public void Install(ProjectConfiguration configuration, TemplateContext context)
    {
        var directory = context.Renderer.GetFolderPath(AppLayerConstant.Services);
        var template = Path.Combine("templates", $"{configuration.Architecture}", directory, "Messaging", "RabbitMqService.cs.txt");
        var output = Path.Combine(context.TargetDirectory, "src", directory, "Messaging", "RabbitMqService.cs");

        context.Render(template, output);
        context.AddPackage("RabbitMQ.Client");
    }
}
