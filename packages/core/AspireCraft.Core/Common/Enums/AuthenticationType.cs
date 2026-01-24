using AspireCraft.Core.Common.Extensions;

namespace AspireCraft.Core.Common.Enums;

public enum AuthenticationType
{
    [DisplayName("JWT")]
    Jwt,

    [DisplayName("Auth0")]
    Auth0,

    [DisplayName("Duende Identity")]
    DuendeIdentity,

    [DisplayName("None")]
    None
}
