using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace AspireCraft.Core.Extensions;

public static class EnumExtensions
{
    public static string GetDisplayName(this Enum value)
    {
        var attr = value.GetType()
            .GetField(value.ToString())?
            .GetCustomAttribute<DisplayAttribute>();

        return attr?.Name ?? value.ToString();
    }

    public static string[] GetEnumValues<TEnum>() where TEnum : Enum
    {
        return Enum.GetValues(typeof(TEnum))
            .Cast<Enum>()
            .Select(e =>
            {
                var member = typeof(TEnum).GetMember(e.ToString()).FirstOrDefault();
                var displayAttr = member?.GetCustomAttribute<DisplayAttribute>();

                return displayAttr?.Name ?? e.ToString();
            })
            .ToArray();
    }

    public static string GetShortName(this Enum value)
    {
        var attr = value.GetType()
            .GetMember(value.ToString())
            .FirstOrDefault()?
            .GetCustomAttribute<DisplayAttribute>();

        return attr?.GetShortName() ?? string.Empty;
    }
}
