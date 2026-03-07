using System.ComponentModel.DataAnnotations;

namespace AspireCraft.Core.Enums;

public enum TestProject
{
    [Display(Name = "Unit Tests")]
    UnitTests,

    [Display(Name = "Integration Tests")]
    IntegrationTests,

    [Display(Name = "Architecture Tests")]
    ArchitectureTests
}
