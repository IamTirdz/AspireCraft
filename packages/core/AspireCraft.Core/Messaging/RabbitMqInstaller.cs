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

        var projectDir = Path.Combine(context.TargetDirectory, "src", $"{context.ProjectName}.Infrastructure");
        var messagingDir = Path.Combine(projectDir, directory, "Messaging");

        var template = Path.Combine("Integrations", "Messaging", "RabbitMqService.cs.tpl");
        var output = Path.Combine(messagingDir, "RabbitMqService.cs");

        context.Render(template, output);
        context.AddPackage("RabbitMQ.Client", projectDir);
    }
}
