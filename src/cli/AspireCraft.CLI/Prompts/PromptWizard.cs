using AspireCraft.Core.Enums;
using AspireCraft.Core.Models;
using Spectre.Console;

namespace AspireCraft.CLI.Prompts;

public sealed class PromptWizard
{
    public Dictionary<string, object> Ask(List<PromptDefinition> prompts)
    {
        var answers = new Dictionary<string, object>();
        foreach (var prompt in prompts)
        {
            AnsiConsole.MarkupLine($"[grey]│[/] ");
            AnsiConsole.MarkupLine($"[blue]◇[/] [white]{Markup.Escape(prompt.Question)}[/]");

            object answer;
            switch (prompt.Type)
            {
                case PromptType.Text:
                    var input = AnsiConsole.Prompt(
                        new TextPrompt<string>("[grey]│[/] ")
                        .PromptStyle("deepSkyBlue2")
                        .Validate(val => string.IsNullOrWhiteSpace(val)
                            ? ValidationResult.Error("Value cannot be empty")
                            : ValidationResult.Success()));
                    answer = input ?? prompt.DefaultValue ?? string.Empty;
                    break;

                case PromptType.Boolean:
                    var defaultValue = prompt.DefaultValue as bool? ?? false;
                    var options = defaultValue ? new[] { "Yes", "No" } : new[] { "No", "Yes" };
                    var selection = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                        .HighlightStyle("deepSkyBlue3_1")
                        .AddChoices(options));
                    answer = selection == "Yes";
                    AnsiConsole.MarkupLine($"[grey]│[/] [deepSkyBlue2]{selection}[/]");
                    break;

                case PromptType.SingleSelect when prompt.Choices != null:
                    var result = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                        .HighlightStyle("deepSkyBlue3_1")
                        .AddChoices(prompt.Choices));
                    answer = result ?? prompt.DefaultValue ?? string.Empty;
                    AnsiConsole.MarkupLine($"[grey]│[/] [deepSkyBlue2]{Markup.Escape(result!)}[/]");
                    break;

                case PromptType.MultiSelect when prompt.Choices != null:
                    var results = AnsiConsole.Prompt(
                        new MultiSelectionPrompt<string>()
                        .HighlightStyle("deepSkyBlue3_1")
                        .AddChoices(prompt.Choices));
                    var selectedOption = results.Any() ? results : (prompt.DefaultValue as IEnumerable<string> ?? new List<string>());
                    answer = selectedOption;
                    AnsiConsole.MarkupLine($"[grey]│[/] [deepSkyBlue2]{Markup.Escape(string.Join(", ", selectedOption))}[/]");
                    break;

                default:
                    throw new InvalidOperationException($"Invalid prompt configuration for {prompt.Key}");
            }

            answers[prompt.Key] = answer;
        }

        return answers;
    }

    public ProjectContext GetInputs(Dictionary<string, object> dict)
    {
        var modules = new[] { "cache", "messaging", "storage", "email", "sms", "payment" };

        return new ProjectContext
        {
            ProjectName = GetVal<string>(dict, "projectName"),
            Template = GetVal<string>(dict, "architecture"),
            Framework = GetVal<string>(dict, "framework"),
            Database = GetVal<string>(dict, "database"),
            Authentication = GetVal<string>(dict, "authentication"),
            Modules = modules.Select(key => GetVal<string>(dict, key)).ToList(),
            TestProjects = GetVal<List<string>>(dict, "testProject") ?? new(),
            MockLibrary = GetVal<string>(dict, "mockLib")
        };
    }

    private T GetVal<T>(Dictionary<string, object> dict, string key)
    {
        if (dict.TryGetValue(key, out var value) && value is T typedValue)
        {
            return typedValue;
        }

        return default!;
    }
}
