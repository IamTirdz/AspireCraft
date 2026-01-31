using AspireCraft.Core.Common.Abstractions;
using AspireCraft.Core.Common.Constants;
using AspireCraft.Core.Common.Enums;
using AspireCraft.Core.Common.Models;
using AspireCraft.Core.Renderers;

namespace AspireCraft.Core.Storage;

public sealed class AwsS3BucketInstaller : IPackageInstaller
{
    public bool CanInstall(ProjectConfiguration configuration)
    {
        return configuration.Integrations.Contains(IntegrationType.AwsS3Bucket);
    }

    public void Install(ProjectConfiguration configuration, TemplateContext context)
    {
        var directory = context.Renderer.GetFolderPath(AppLayerConstant.Services);

        var projectDir = Path.Combine(context.TargetDirectory, "src", $"{context.ProjectName}.Infrastructure");
        var storageDir = Path.Combine(projectDir, directory, "Storage");

        var template = Path.Combine("Integrations", "Storage", "AwsS3BucketService.cs.tpl");
        var output = Path.Combine(storageDir, "AwsS3BucketService.cs");

        context.Render(template, output);
        context.AddPackage("AWSSDK.S3", projectDir);
    }
}
