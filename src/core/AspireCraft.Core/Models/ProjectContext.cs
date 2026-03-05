namespace AspireCraft.Core.Models;

public sealed class ProjectContext
{
    public string Name { get; init; } = string.Empty;
    public string Template { get; init; } = string.Empty;
    public string Framework { get; init; } = string.Empty;
    public string Database { get; init; } = string.Empty;
    public string Authentication { get; init; } = string.Empty;
    public List<string> Modules { get; init; } = new();
    public List<string> TestProjects { get; init; } = new();
    public string MockLibrary { get; init; } = string.Empty;
}
