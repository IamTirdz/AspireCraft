using AspireCraft.Core.Enums;

namespace AspireCraft.Core.Models;

public sealed class PromptDefinition
{
    public PromptDefinition(string key, string question, PromptType type, string[]? options = null, object? defaultValue = null)
    {
        Key = key;
        Question = question;
        Type = type;
        Choices = options;
        DefaultValue = defaultValue;
    }

    public string Key { get; init; }
    public string Question { get; init; }
    public PromptType Type { get; init; }
    public string[]? Choices { get; init; }
    public object? DefaultValue { get; init; }
}
