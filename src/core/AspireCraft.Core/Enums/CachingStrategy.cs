using System.ComponentModel.DataAnnotations;

namespace AspireCraft.Core.Enums;

public enum CachingStrategy
{
    [Display(Name = "None")]
    None,

    [Display(Name = "Redis Cache")]
    Redis,

    [Display(Name = "In-Memory")]
    InMemory
}
