using AspireCraft.Core.Common.Constants;
using AspireCraft.Core.Common.Enums;

namespace AspireCraft.Core.Common.Models;

public sealed class ProjectConfiguration
{
    public string ProjectName { get; set; } = string.Empty;
    public ArchitectureType Architecture { get; set; }
    public NetFramework Framework { get; set; } = NetFramework.Net8;
    public AuthenticationType? Authentication { get; set; }
    public bool? UseControllers { get; set; } // web api | minimal api
    public string DbContextName { get; set; } = AppConstant.DbContextName;
    public DatabaseProvider Database { get; set; }

    public bool UseNetAspire { get; set; }
    public List<IntegrationType> Integrations { get; set; } = new();

    public bool IncludeUnitTests { get; set; } = false;
    public bool IncludeIntegrationTests { get; set; } = false;
    public bool IncludeArchitectureTests { get; set; } = false;
}
