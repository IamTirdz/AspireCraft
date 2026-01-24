using AspireCraft.Core.Common.Extensions;

namespace AspireCraft.Core.Common.Enums;

public enum StorageProvider
{
    [DisplayName("Azure Blob")]
    AzureBlob,

    [DisplayName("S3 Bucket")]
    AwsS3Bucket,
}
