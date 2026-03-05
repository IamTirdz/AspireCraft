using System.ComponentModel.DataAnnotations;

namespace AspireCraft.Core.Enums;

public enum StorageProvider
{
    [Display(Name = "None")]
    None,

    [Display(Name = "Azure Blob Storage")]
    AzureBlob,

    [Display(Name = "AWS S3")]
    AwsS3
}
