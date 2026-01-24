using AspireCraft.CLI.Common.Enums;

namespace AspireCraft.CLI.Common.Models;

public class ProjectConfiguration
{
    public string ProjectName { get; set; } = string.Empty;
    public ArchitectureType Architecture { get; set; }
    public string Framework { get; set; } = string.Empty;
    public AuthenticationType Authentication { get; set; }
    public bool UseControllers { get; set; } // web api | minimal api
    public string DbContextName { get; set; } = string.Empty;
    public DatabaseProvider Database { get; set; }

    public bool UseNetAspire { get; set; }
    public List<IntegrationType> Integrations { get; set; } = new();

    public bool IncludeUnitTests { get; set; } = false;
    public bool IncludeIntegrationTests { get; set; } = false;
    public bool IncludeArchitectureTests { get; set; } = false;
}