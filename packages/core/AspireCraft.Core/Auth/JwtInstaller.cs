using AspireCraft.Core.Common.Abstractions;
using AspireCraft.Core.Common.Constants;
using AspireCraft.Core.Common.Enums;
using AspireCraft.Core.Common.Models;
using AspireCraft.Core.Renderers;

namespace AspireCraft.Core.Auth;

public sealed class JwtInstaller : IPackageInstaller
{
    public bool CanInstall(ProjectConfiguration configuration)
    {
        return configuration.Integrations.Contains(IntegrationType.Jwt);
    }

    public void Install(ProjectConfiguration configuration, TemplateContext context)
    {
        var directory = context.Renderer.GetFolderPath(AppLayerConstant.Services);
        var template = Path.Combine("templates", $"{configuration.Architecture}", directory, "Auth", "JwtService.cs.txt");
        var output = Path.Combine(context.TargetDirectory, "src", directory, "Auth", "JwtService.cs");

        context.Render(template, output);
        context.AddPackage("Microsoft.AspNetCore.Authentication.JwtBearer");
    }
}
