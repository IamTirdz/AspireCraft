using AspireCraft.Core.Models;
using AspireCraft.Runtime;

namespace AspireCraft.Builder.Generators;

public sealed class ReferenceGenerator
{
    public void ApplyReferences(ProjectContext context, TemplateDefinition template)
    {
        var root = Path.GetDirectoryName(context.SolutionPath)!;
        var src = Path.Combine(root, template.SrcFolder);

        foreach (var reference in template.References)
        {
            var fromProj = Path.Combine(src,
                $"{context.ProjectName}.{reference.From}",
                $"{context.ProjectName}.{reference.From}.csproj");

            var toProj = Path.Combine(src,
                $"{context.ProjectName}.{reference.To}",
                $"{context.ProjectName}.{reference.To}.csproj");

            DotnetRunner.Run(
                $"add \"{fromProj}\" reference \"{toProj}\"",
                Path.GetDirectoryName(fromProj)!);
        }
    }
}
