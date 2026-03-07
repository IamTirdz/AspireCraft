using AspireCraft.Core.Models;
using AspireCraft.Runtime;

namespace AspireCraft.Builder.Generators;

public sealed class ReferenceGenerator
{
    public void ApplyReferences(ProjectContext context, TemplateDefinition template)
    {
        foreach (var reference in template.Dependencies)
        {
            var projectPath = context.ProjectPath[reference.Project];
            var dependencyPath = context.ProjectPath[reference.Dependency];

            var dependencyProj = Directory.GetFiles(dependencyPath, "*.csproj").FirstOrDefault();

            DotnetRunner.Run($"add reference \"{dependencyProj}\"", projectPath);
        }
    }
}
