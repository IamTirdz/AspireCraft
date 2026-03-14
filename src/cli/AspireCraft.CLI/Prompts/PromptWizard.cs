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

            switch (prompt)
            {
                case PromptDefinition<string> p when p.Type == PromptType.Text:
                    answers[p.Key] = AskText(p);
                    break;

                case PromptDefinition<bool> p when p.Type == PromptType.Boolean:
                    answers[p.Key] = AskBoolean(p);
                    break;

                case PromptDefinition<string> p when p.Type == PromptType.SingleSelect:
                    answers[p.Key] = AskSingleSelect(p);
                    break;

                case PromptDefinition<string> p when p.Type == PromptType.MultiSelect:
                    answers[p.Key] = AskMultiSelect(p);
                    break;

                default:
                    throw new InvalidOperationException($"Invalid prompt configuration for {prompt.Key}");
            }
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
            IncludeIntegrationTest = GetVal<bool>(dict, "integrationTest"),
            IncludeArchitectureTest = GetVal<bool>(dict, "architectureTest"),
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

    private static string AskText(PromptDefinition<string> p)
    {
        var prompt = new TextPrompt<string>("[grey]│[/] ")
            .PromptStyle("deepSkyBlue2")
            .Validate(val =>
                string.IsNullOrWhiteSpace(val)
                    ? ValidationResult.Error("Value cannot be empty")
                    : ValidationResult.Success());

        if (!string.IsNullOrWhiteSpace(p.DefaultValue))
        {
            prompt.DefaultValue(p.DefaultValue);
        }

        return AnsiConsole.Prompt(prompt);
    }

    private static bool AskBoolean(PromptDefinition<bool> p)
    {
        var choices = p.DefaultValue
            ? new[] { true, false }
            : new[] { false, true };

        var prompt = new SelectionPrompt<bool>()
            .HighlightStyle("deepSkyBlue3_1")
            .WrapAround()
            .AddChoices(choices)
            .UseConverter(v => v ? "Yes" : "No");

        var result = AnsiConsole.Prompt(prompt);
        AnsiConsole.MarkupLine($"[grey]│[/] [deepSkyBlue2]{(result ? "Yes" : "No")}[/]");

        return result;
    }

    private static string AskSingleSelect(PromptDefinition<string> p)
    {
        var choices = p.Choices
            .Where(c => !p.DisabledChoices.Contains(c))
            .Distinct()
            .ToList();

        if (choices.Count == 1)
        {
            var selected = choices[0];
            AnsiConsole.MarkupLine($"[grey]│[/] [deepSkyBlue2]{selected}[/]");
            return selected;
        }

        if (p.DefaultValue != null && choices.Contains(p.DefaultValue))
        {
            choices.Remove(p.DefaultValue);
            choices.Insert(0, p.DefaultValue);
        }

        var prompt = new SelectionPrompt<string>()
            .HighlightStyle("deepSkyBlue3_1")
            .WrapAround()
            .AddChoices(choices)
            .UseConverter(c => p.DisabledChoices!.Contains(c) ? $"[grey]{c} (Disabled)[/]" : c);

        var result = AnsiConsole.Prompt(prompt);
        AnsiConsole.MarkupLine($"[grey]│[/] [deepSkyBlue2]{Markup.Escape(result)}[/]");

        return result;
    }

    private static List<string> AskMultiSelect(PromptDefinition<string> p)
    {
        var choices = p.Choices
            .Where(c => !p.DisabledChoices.Contains(c))
            .Distinct()
            .ToList();

        if (choices.Count == 1)
        {
            var selected = choices[0];
            AnsiConsole.MarkupLine($"[grey]│[/] [deepSkyBlue2]{selected}[/]");
            return new List<string> { selected };
        }

        var prompt = new MultiSelectionPrompt<string>()
            .HighlightStyle("deepSkyBlue3_1")
            .WrapAround()
            .AddChoices(choices)
            .UseConverter(c => p.DisabledChoices!.Contains(c) ? $"[grey]{c} (Disabled)[/]" : c);

        if (p.DefaultValues != null)
        {
            foreach (var value in p.DefaultValues.Where(choices.Contains))
            {
                prompt.Select(value);
            }
        }

        var result = AnsiConsole.Prompt(prompt).ToList();
        AnsiConsole.MarkupLine($"[grey]│[/] [deepSkyBlue2]{Markup.Escape(string.Join(", ", result))}[/]");

        return result;
    }
}
