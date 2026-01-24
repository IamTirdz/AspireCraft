using AspireCraft.CLI.Common.Extensions;

namespace AspireCraft.CLI.Common.Enums;

public enum StorageProvider
{
    [DisplayName("Azure Blob")]
    AzureBlob,

    [DisplayName("S3 Bucket")]
    AwsS3Bucket,
}
