using AspireCraft.CLI.Common.Extensions;

namespace AspireCraft.CLI.Common.Enums;

public enum ArchitectureType
{
    [DisplayName("Clean Architecture")]
    CleanArchitecture,

    [DisplayName("N-Layer")]
    NLayer,

    [DisplayName("Serverless")]
    Serverless,
}
