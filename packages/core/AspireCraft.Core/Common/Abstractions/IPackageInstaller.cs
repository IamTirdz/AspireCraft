using AspireCraft.Core.Common.Models;
using AspireCraft.Core.Renderers;

namespace AspireCraft.Core.Common.Abstractions;

public interface IPackageInstaller
{
    bool CanInstall(ProjectConfiguration configuration);
    void Install(TemplateContext context);
}
