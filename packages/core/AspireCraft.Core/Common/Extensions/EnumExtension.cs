using System.Reflection;
using System.Text.RegularExpressions;

namespace AspireCraft.Core.Common.Extensions;

public static class EnumExtension
{
    public static string ToEnumValue(this Enum value)
    {
        FieldInfo fi = value.GetType().GetField(value.ToString())!;
        var attr = fi.GetCustomAttribute<DisplayNameAttribute>();
        if (attr != null) return attr.Name;

        string name = value.ToString();

        name = Regex.Replace(name, @"\b([A-Z])([A-Z][a-z])", "$1-$2");
        name = Regex.Replace(name, "(?<=[a-z])([A-Z])", " $1");

        return name;
    }

    public static string[] ToEnumList(this Array enumValues)
    {
        return enumValues.Cast<Enum>()
            .Select(e => e.ToEnumValue())
            .ToArray();
    }

    public static T FromEnumValue<T>(this string displayName) where T : Enum
    {
        foreach (var field in typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static))
        {
            var attr = field.GetCustomAttribute<DisplayNameAttribute>();
            if (attr?.Name == displayName)
                return (T)field.GetValue(null)!;

            // fallback to formatted enum name
            var formatted = ((Enum)field.GetValue(null)!).ToEnumValue();
            if (formatted == displayName)
                return (T)field.GetValue(null)!;
        }

        throw new ArgumentException($"'{displayName}' is not a valid value of {typeof(T).Name}");
    }
}

[AttributeUsage(AttributeTargets.Field)]
public sealed class DisplayNameAttribute : Attribute
{
    public string Name { get; }
    public DisplayNameAttribute(string name) => Name = name;
}
