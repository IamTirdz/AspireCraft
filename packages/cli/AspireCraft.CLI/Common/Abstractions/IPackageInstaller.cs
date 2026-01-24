using AspireCraft.CLI.Common.Models;
using AspireCraft.CLI.Renderers;

namespace AspireCraft.CLI.Common.Abstractions;

public interface IPackageInstaller
{
    bool CanInstall(ProjectConfiguration configuration);
    void Install(GenerationContext context);
}
