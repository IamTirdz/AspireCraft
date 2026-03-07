using AspireCraft.Core.Enums;
using AspireCraft.Core.Models;
using AspireCraft.Runtime;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace AspireCraft.Builder.Generators;

public sealed class ProjectGenerator
{
    public void Generate(ProjectContext context, TemplateDefinition template)
    {
        var framework = Enum.GetValues<TargetFramework>()
            .Select(f => typeof(TargetFramework).GetField(f.ToString())?
            .GetCustomAttribute<DisplayAttribute>())
            .FirstOrDefault(a => a?.Name == context.Framework)?.ShortName ?? "net8.0";

        foreach (var project in template.Projects)
        {
            var name = project.Name.Replace("{{ProjectName}}", context.ProjectName);
            var path = project.Path.Replace("{{ProjectName}}", context.ProjectName);

            var root = Path.GetDirectoryName(context.SolutionPath)!;
            var fullPath = Path.Combine(root, path.Replace("/", Path.DirectorySeparatorChar.ToString()));

            if (SkipProject(name, context))
            {
                continue;
            }

            Directory.CreateDirectory(fullPath);
            DotnetRunner.Run($"new {project.Type} -n {name} -f {framework} -o .", fullPath);

            var csproj = Path.Combine(fullPath, $"{name}.csproj");
            DotnetRunner.Run($"sln add {csproj}", root);

            var shortName = name.Replace($"{context.ProjectName}.", "");
            context.ProjectPath[shortName] = fullPath;
        }
    }

    private bool SkipProject(string projectName, ProjectContext context)
    {
        if (projectName.Contains("IntegrationTests") && !context.IncludeIntegrationTest)
            return true;

        if (projectName.Contains("ArchitectureTests") && !context.IncludeArchitectureTest)
            return true;

        return false;
    }
}
