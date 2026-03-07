using AspireCraft.Core.Enums;

namespace AspireCraft.Core.Models;

public abstract class PromptDefinition
{
    public string Key { get; set; }
    public string Question { get; set; }
    public PromptType Type { get; set; }

    protected PromptDefinition(string key, string question, PromptType type)
    {
        Key = key;
        Question = question;
        Type = type;
    }
}

public sealed class PromptDefinition<T> : PromptDefinition
{
    public T? DefaultValue { get; set; }
    public IEnumerable<T>? DefaultValues { get; set; }
    public T[]? Choices { get; set; }
    public T[]? DisabledChoices { get; set; }

    public PromptDefinition(string key, string question, PromptType type, T[]? options = null, T? defaultValue = default, IEnumerable<T>? defaultValues = null, T[]? disabledOptions = null)
         : base(key, question, type)
    {
        DefaultValue = defaultValue;
        DefaultValues = defaultValues;
        Choices = options ?? Array.Empty<T>();
        DisabledChoices = disabledOptions ?? Array.Empty<T>();
    }
}
