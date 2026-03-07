using System.ComponentModel.DataAnnotations;

namespace AspireCraft.Core.Enums;

public enum MockLibrary
{
    [Display(Name = "Moq")]
    Moq,

    [Display(Name = "NSubstitute")]
    NSubstitute
}
