namespace AspireCraft.Core.Models;

public sealed class TemplateDefinition
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public List<ProjectDefinition> Projects { get; set; } = [];
    public List<DependencyDefinition> Dependencies { get; set; } = [];
    public List<ServiceDefinition> Services { get; set; } = [];
}

public sealed class ProjectDefinition
{
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
}

public sealed class DependencyDefinition
{
    public string Project { get; set; } = string.Empty;
    public string Dependency { get; set; } = string.Empty;
}

public sealed class ServiceDefinition
{
    public string Name { get; set; } = "";
    public string Category { get; set; } = "";
    public string Provider { get; set; } = "";
    public string Project { get; set; } = "";
    public string Template { get; set; } = "";
    public string Condition { get; set; } = "";
    public List<string> Packages { get; set; } = [];
}
