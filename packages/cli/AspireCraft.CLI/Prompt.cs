using AspireCraft.Core.Common.Constants;
using AspireCraft.Core.Common.Extensions;

namespace AspireCraft.CLI;

public static class Prompt
{
    public static string Ask(string label)
    {
        return AppConsole.Prompt(label);
    }

    public static bool Confirm(string label, bool defaultValue = false)
    {
        return AppConsole.Prompt(label, defaultValue);
    }

    public static TEnum Select<TEnum>(string label) where TEnum : Enum
    {
        return AppConsole.SelectionPrompt(label, Enum.GetValues(typeof(TEnum)).ToEnumList())
            .FromEnumValue<TEnum>();
    }

    public static IReadOnlyList<TEnum> MultiSelect<TEnum>(string label, IEnumerable<TEnum> options, bool includeNone = true) where TEnum : Enum
    {
        var values = options.Select(o => o.ToString()).ToList();

        if (includeNone)
            values.Insert(0, AppConstant.NoneOption);

        var selected = AppConsole.MultiSelectionPrompt(label, values);

        if (selected.Contains(AppConstant.NoneOption))
            return Array.Empty<TEnum>();

        return selected
            .Select(s => s.FromEnumValue<TEnum>())
            .ToList();
    }
}
