using AspireCraft.Core.Common.Abstractions;
using AspireCraft.Core.Common.Constants;
using AspireCraft.Core.Common.Enums;
using AspireCraft.Core.Common.Models;
using AspireCraft.Core.Renderers;

namespace AspireCraft.Core.Auth;

public class DuendeIdentityInstaller : IPackageInstaller
{
    public bool CanInstall(ProjectConfiguration configuration)
    {
        return configuration.Integrations.Contains(IntegrationType.DuendeIdentity);
    }

    public void Install(ProjectConfiguration configuration, TemplateContext context)
    {
        var directory = context.Renderer.GetFolderPath(AppLayerConstant.Services);

        var projectDir = Path.Combine(context.TargetDirectory, "src", $"{context.ProjectName}.Infrastructure");
        var authDir = Path.Combine(projectDir, directory, "Auth");

        var template = Path.Combine("Integrations", "Auth", "DuendeIdentityService.cs.tpl");
        var output = Path.Combine(authDir, "DuendeIdentityService.cs");

        context.Render(template, output);
        context.AddPackage("Duende.IdentityServer", projectDir);
    }
}
