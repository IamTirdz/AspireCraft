using Spectre.Console;

namespace AspireCraft.CLI.Common.Extensions;

/// <summary>
/// AnsiConsole extension
/// </summary>
public static class AppConsole
{
    private const string BaseColor = "grey";
    private const string TextColor = "white";
    private const string PrimaryColor = "blue";

    public static void WriteLine(string? text = null, bool isStart = false, bool isEnd = false)
    {
        if (isStart)
        {
            AnsiConsole.MarkupLine($"[{BaseColor}]┌[/] {text}");
        }
        else if (isEnd)
        {
            AnsiConsole.MarkupLine($"[{BaseColor}]└[/] {text}");
        }
        else
        {
            AnsiConsole.MarkupLine($"[{BaseColor}]│[/] {text}");
        }
    }

    public static string Prompt(string text)
    {
        WriteLine();
        AnsiConsole.MarkupLine($"[{PrimaryColor}]◇[/][{TextColor}] {Markup.Escape(text)}[/]");

        var result = AnsiConsole.Prompt(
            new TextPrompt<string>($"[{BaseColor}]│[/]  ")
                .PromptStyle(BaseColor)
        );

        return result;
    }

    public static bool Prompt(string text, bool? defaultValue)
    {
        WriteLine();
        AnsiConsole.MarkupLine($"[{PrimaryColor}]◇[/][{TextColor}] {Markup.Escape(text)}[/]");

        var options = defaultValue.HasValue
            ? new[] { "Yes", "No" }   // default is Yes
            : new[] { "No", "Yes" };  // default is No

        var selection = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .AddChoices(options)
        );

        var result = selection == "Yes";

        AnsiConsole.MarkupLine($"[{BaseColor}]│[/] [{BaseColor}]{(result ? "Yes" : "No")}[/]");
        return result;
    }

    public static string SelectionPrompt(string text, IEnumerable<string> choices)
    {
        WriteLine();
        AnsiConsole.MarkupLine($"[{PrimaryColor}]◇[/][{TextColor}] {Markup.Escape(text)}[/]");

        var choiceList = choices.ToList();
        string selection;
        if (choiceList.Count == 1)
        {
            selection = choiceList[0];
        }
        else
        {
            selection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .AddChoices(choiceList)
            );
        }

        AnsiConsole.MarkupLine($"[{BaseColor}]│[/] [{BaseColor}]{selection}[/]");
        return selection;
    }

    public static List<string> MultiSelectionPrompt(string text, IEnumerable<string> choices)
    {
        WriteLine();
        AnsiConsole.MarkupLine($"[{PrimaryColor}]◇[/][{TextColor}] {Markup.Escape(text)}[/]");

        var choiceList = choices.ToList();
        if (choiceList.Count == 1)
        {
            var selection = choiceList[0];
            AnsiConsole.MarkupLine($"[{BaseColor}]│[/][{BaseColor}] {selection}[/]");
            return new List<string> { selection };
        }
        else
        {
            var selections = AnsiConsole.Prompt(
                new MultiSelectionPrompt<string>()
                    .AddChoices(choiceList)
            );

            AnsiConsole.MarkupLine($"[{BaseColor}]│[/][{BaseColor}] {string.Join(", ", selections)}[/]");
            return selections.ToList();
        }
    }
}
