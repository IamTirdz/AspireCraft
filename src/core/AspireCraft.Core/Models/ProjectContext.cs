namespace AspireCraft.Core.Models;

public sealed class ProjectContext
{
    public string ProjectName { get; set; } = string.Empty;
    public string Template { get; set; } = string.Empty;
    public string Framework { get; set; } = string.Empty;
    public string Database { get; set; } = string.Empty;
    public string Authentication { get; set; } = string.Empty;
    public List<string> Modules { get; set; } = [];
    public List<string> TestProjects { get; set; } = [];
    public string MockLibrary { get; set; } = string.Empty;

    public string SolutionPath { get; set; } = string.Empty;
    public Dictionary<string, string> ProjectPath { get; set; } = [];
}
