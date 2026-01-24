using AspireCraft.Core.Common.Abstractions;
using AspireCraft.Core.Common.Enums;
using AspireCraft.Core.Common.Models;
using AspireCraft.Core.Renderers;

namespace AspireCraft.Core.Storage;

public sealed class AzureBlobInstaller : IPackageInstaller
{
    public bool CanInstall(ProjectConfiguration configuration)
    {
        return configuration.Integrations.Contains(IntegrationType.AzureBlob);
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

        var template = Path.Combine("templates", $"{configuration.Architecture}", directory, "Storage", "AzureBlobService.cs.txt");
        var output = Path.Combine(context.RootPath, "src", directory, "Storage", "AzureBlobService.cs");

        context.Render(template, output);
        context.AddPackage("Azure.Storage.Blobs");
    }
}
