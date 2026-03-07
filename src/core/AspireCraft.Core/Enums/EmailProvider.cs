using System.ComponentModel.DataAnnotations;

namespace AspireCraft.Core.Enums;

public enum EmailProvider
{
    [Display(Name = "None")]
    None,

    [Display(Name = "SendGrid")]
    SendGrid,

    [Display(Name = "Mailgun")]
    Mailgun
}
