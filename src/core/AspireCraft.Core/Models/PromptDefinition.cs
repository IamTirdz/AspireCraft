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

    public string Key { get; set; }
    public string Question { get; set; }
    public PromptType Type { get; set; }
    public string[]? Choices { get; set; }
    public object? DefaultValue { get; set; }
}
