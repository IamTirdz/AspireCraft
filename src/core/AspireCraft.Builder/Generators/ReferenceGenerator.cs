using AspireCraft.Core.Models;
using AspireCraft.Runtime;

namespace AspireCraft.Builder.Generators;

public sealed class ReferenceGenerator
{
    public void ApplyReferences(ProjectContext context, TemplateDefinition template)
    {
        foreach (var reference in template.Dependencies)
        {
            if (!context.ProjectPath.TryGetValue(reference.Project, out var projectPath) ||
                !context.ProjectPath.TryGetValue(reference.Dependency, out var dependencyPath))
            {
                continue;
            }

            var dependencyProj = Directory.GetFiles(dependencyPath, "*.csproj").FirstOrDefault();

            DotnetRunner.Run($"add reference \"{dependencyProj}\"", projectPath);
        }
    }
}
