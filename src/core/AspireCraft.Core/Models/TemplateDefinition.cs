namespace AspireCraft.Core.Models;

public sealed class TemplateDefinition
{
    public string Name { get; set; } = string.Empty;
    public string SrcFolder { get; set; } = "src";
    public string TestFolder { get; set; } = "tests";

    public List<LayerDefinition> Layers { get; set; } = [];
    public List<ProjectReference> References { get; set; } = [];
    public List<TestDefinition> Tests { get; set; } = [];
}

public sealed class LayerDefinition
{
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
}

public sealed class ProjectReference
{
    public string From { get; set; } = string.Empty;
    public string To { get; set; } = string.Empty;
}

public sealed class TestDefinition
{
    public string Name { get; set; } = string.Empty;
    public string Target { get; set; } = string.Empty;
}
