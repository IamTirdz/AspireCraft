using AspireCraft.Core.Common.Abstractions;
using AspireCraft.Core.Common.Enums;
using AspireCraft.Core.Common.Models;
using AspireCraft.Core.Renderers;

namespace AspireCraft.Core.Cache;

public sealed class RedisCacheInstaller : IPackageInstaller
{
    public bool CanInstall(ProjectConfiguration configuration)
    {
        return configuration.Integrations.Contains(IntegrationType.Redis);
    }

    public void Install(ProjectConfiguration configuration, TemplateContext context)
    {
        string directory = configuration.Architecture switch
        {
            ArchitectureType.CleanArchitecture => "Infrastructure/Services",
            ArchitectureType.NLayer => "Data",
            ArchitectureType.Serverless => "Infrastructure/Services",
            _ => throw new NotSupportedException("Unsupported architecture")
        };

        var template = Path.Combine("templates", $"{configuration.Architecture}", directory, "Cache", "RedisCacheService.cs.txt");
        var output = Path.Combine(context.RootPath, "src", directory, "Cache", "RedisCacheService.cs");

        context.Render(template, output);
        context.AddPackage("StackExchange.Redis");
    }
}
