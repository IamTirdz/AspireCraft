using AspireCraft.Core.Common.Abstractions;
using AspireCraft.Core.Common.Constants;
using AspireCraft.Core.Common.Enums;
using AspireCraft.Core.Common.Models;
using AspireCraft.Core.Renderers;

namespace AspireCraft.Core.Sms;

public sealed class TwilioInstaller : IPackageInstaller
{
    public bool CanInstall(ProjectConfiguration configuration)
    {
        return configuration.Integrations.Contains(IntegrationType.Twilio);
    }

    public void Install(ProjectConfiguration configuration, TemplateContext context)
    {
        var directory = context.Renderer.GetFolderPath(AppLayerConstant.Services);

        var projectDir = Path.Combine(context.TargetDirectory, "src", $"{context.ProjectName}.Infrastructure");
        var smsDir = Path.Combine(projectDir, directory, "SMS");

        var template = Path.Combine("Integrations", "SMS", "TwilioService.cs.tple");
        var output = Path.Combine(smsDir, "TwilioService.cs");

        context.Render(template, output);
        context.AddPackage("Twilio", projectDir);
    }
}
