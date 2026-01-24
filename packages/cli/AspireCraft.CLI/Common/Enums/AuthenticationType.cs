using AspireCraft.CLI.Common.Extensions;

namespace AspireCraft.CLI.Common.Enums;

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
