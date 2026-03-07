using System.ComponentModel.DataAnnotations;

namespace AspireCraft.Core.Enums;

public enum AuthenticationType
{
    [Display(Name = "JwtBearer")]
    JwtBearer,

    [Display(Name = "Auth0")]
    Auth0,

    [Display(Name = "Keycloak")]
    Keycloak,

    [Display(Name = "Duende Identity")]
    DuendeIdentity
}
