using System.ComponentModel.DataAnnotations;

namespace AspireCraft.Core.Enums;

public enum TargetFramework
{
    [Display(Name = ".NET 8.0 (LTS)", ShortName = "net8.0")]
    DotNet8,

    [Display(Name = ".NET 9.0", ShortName = "net9.0")]
    DotNet9,

    [Display(Name = ".NET 10.0 (LTS)", ShortName = "net10.0")]
    DotNet10
}
