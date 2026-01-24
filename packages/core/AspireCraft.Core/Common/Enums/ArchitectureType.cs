using AspireCraft.Core.Common.Extensions;

namespace AspireCraft.Core.Common.Enums;

public enum ArchitectureType
{
    [DisplayName("Clean Architecture")]
    CleanArchitecture,

    [DisplayName("N-Layer")]
    NLayer,

    [DisplayName("Serverless")]
    Serverless,
}
