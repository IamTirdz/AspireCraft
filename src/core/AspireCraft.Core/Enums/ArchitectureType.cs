using System.ComponentModel.DataAnnotations;

namespace AspireCraft.Core.Enums;

public enum ArchitectureType
{
    [Display(Name = "Clean Architecture")]
    CleanArchitecture,

    [Display(Name = "Vertical Slice Architecture")]
    VerticalSliceArchitecture,
}
