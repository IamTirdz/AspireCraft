using AspireCraft.CLI.Common.Extensions;

namespace AspireCraft.CLI.Common.Enums;

public enum NetFramework
{
    [DisplayName(".NET 8.0 (LTS)")]
    Net8,

    [DisplayName(".NET 9.0")]
    Net9,

    [DisplayName(".NET 10.0 (preview)")]
    Net10,
}
