using AspireCraft.Core.Common.Abstractions;
using AspireCraft.Core.Common.Constants;
using AspireCraft.Core.Common.Enums;
using AspireCraft.Core.Common.Models;
using AspireCraft.Core.Renderers;

namespace AspireCraft.Core.Mailer.Providers;

public sealed class SendGridInstaller : IPackageInstaller
{
    public bool CanInstall(ProjectConfiguration configuration)
    {
        return configuration.Integrations.Contains(IntegrationType.SendGrid);
    }

    public void Install(ProjectConfiguration configuration, TemplateContext context)
    {
        var directory = context.Renderer.GetFolderPath(AppLayerConstant.Services);

        var projectDir = Path.Combine(context.TargetDirectory, "src", $"{context.ProjectName}.Infrastructure");
        var emailDir = Path.Combine(projectDir, directory, "Email");

        var template = Path.Combine("Integrations", "Email", "SendGridService.cs.tpl");
        var output = Path.Combine(emailDir, "SendGridService.cs");

        context.Render(template, output);
        context.AddPackage("SendGrid", projectDir);
    }
}
