using AspireCraft.Core.Common.Abstractions;
using AspireCraft.Core.Common.Enums;
using AspireCraft.Core.Common.Models;
using AspireCraft.Core.Renderers;

namespace AspireCraft.Core.Cache;

public sealed class InMemoryCacheInstaller : IPackageInstaller
{
    public bool CanInstall(ProjectConfiguration configuration)
    {
        return configuration.Integrations.Contains(IntegrationType.InMemory);
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

        var template = Path.Combine("templates", $"{configuration.Architecture}", directory, "Cache", "InMemoryCacheService.cs.txt");
        var output = Path.Combine(context.RootPath, "src", directory, "Cache", "InMemoryCacheService.cs");

        context.Render(template, output);
    }
}
