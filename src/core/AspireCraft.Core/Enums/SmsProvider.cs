using System.ComponentModel.DataAnnotations;

namespace AspireCraft.Core.Enums;

public enum SmsProvider
{
    [Display(Name = "None")]
    None,

    [Display(Name = "Twillio")]
    Twillio,

    [Display(Name = "8x8 (Wavecell)")]
    EightByEight
}
