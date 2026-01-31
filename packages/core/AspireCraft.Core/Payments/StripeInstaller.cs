using AspireCraft.Core.Common.Abstractions;
using AspireCraft.Core.Common.Constants;
using AspireCraft.Core.Common.Enums;
using AspireCraft.Core.Common.Models;
using AspireCraft.Core.Renderers;

namespace AspireCraft.Core.Payments;

public sealed class StripeInstaller : IPackageInstaller
{
    public bool CanInstall(ProjectConfiguration configuration)
    {
        return configuration.Integrations.Contains(IntegrationType.Stripe);
    }

    public void Install(ProjectConfiguration configuration, TemplateContext context)
    {
        var directory = context.Renderer.GetFolderPath(AppLayerConstant.Services);

        var projectDir = Path.Combine(context.TargetDirectory, "src", $"{context.ProjectName}.Infrastructure");
        var paymentDir = Path.Combine(projectDir, directory, "Payments");

        var template = Path.Combine("Integrations", "Payments", "StripeService.cs.tpl");
        var output = Path.Combine(paymentDir, "StripeService.cs");

        context.Render(template, output);
        context.AddPackage("Stripe.net", projectDir);
    }
}