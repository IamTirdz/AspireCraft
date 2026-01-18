namespace AspireCraft.CLI.Common.Models;

public class ProjectConfiguration
{
    public string ProjectName { get; set; } = string.Empty;
    public string Architecture { get; set; } = string.Empty;
    public string Framework { get; set; } = string.Empty;
    public string AuthenticationType { get; set; } = string.Empty;
    public bool UseControllers { get; set; }
    public string DatabaseContext { get; set; } = string.Empty;
    public string DatabaseProvider { get; set; } = string.Empty;
    public bool UseNetAspire { get; set; }
    public bool UsePolly { get; set; }
    public bool UseSerilog { get; set; }
    public string? Cache { get; set; }
    public List<string> Messaging { get; set; } = new List<string>();
    public string? BackgroundProcess { get; set; }
    public List<string> Testing { get; set; } = new List<string>();
}