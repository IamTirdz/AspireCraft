using AspireCraft.Core.Common.Enums;

namespace AspireCraft.Core.Common.Extensions;

public static class NetFrameworkExtensions
{
    public static string ToTargetFramework(this NetFramework framework)
    {
        return framework switch
        {
            NetFramework.Net8 => "net8.0",
            NetFramework.Net9 => "net9.0",
            NetFramework.Net10 => "net10.0",
            _ => throw new ArgumentOutOfRangeException(nameof(framework), framework, null)
        };
    }
}
