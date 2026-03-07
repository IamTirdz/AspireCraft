using System.ComponentModel.DataAnnotations;

namespace AspireCraft.Core.Enums;

public enum PaymentProvider
{
    [Display(Name = "None")]
    None,

    [Display(Name = "PayPal")]
    PayPal,

    [Display(Name = "Stripe")]
    Stripe
}
