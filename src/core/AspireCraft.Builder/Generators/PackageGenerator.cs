using AspireCraft.Core.Enums;
using AspireCraft.Core.Models;
using AspireCraft.Runtime;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace AspireCraft.Builder.Generators;

public sealed class PackageGenerator
{
    public void Generate(ProjectContext context, TemplateDefinition template)
    {
        var framework = Enum.GetValues<TargetFramework>()
           .Select(f => typeof(TargetFramework).GetField(f.ToString())?
           .GetCustomAttribute<DisplayAttribute>())
           .FirstOrDefault(a => a?.Name == context.Framework)?.ShortName ?? "net8.0";

        var versionPrefix = framework.Replace("net", "");

        foreach (var project in template.Projects)
        {
            var name = project.Name.Replace("{{ProjectName}}", context.ProjectName);
            var path = project.Path.Replace("{{ProjectName}}", context.ProjectName);
            var projectIdentifier = name.Replace($"{context.ProjectName}.", "");

            var root = Path.GetDirectoryName(context.SolutionPath)!;
            var fullPath = Path.Combine(root, path.Replace("/", Path.DirectorySeparatorChar.ToString()));

            if (HasSkipProject(name, context))
            {
                continue;
            }

            var packageConfig = template.Packages.FirstOrDefault(p => p.Project == projectIdentifier);
            if (packageConfig != null)
            {
                foreach (var package in packageConfig.Packages)
                {
                    var command = $"add package {package}";
                    if (package.StartsWith("Microsoft."))
                    {
                        command = $"add package {package} -v {versionPrefix}.*";
                    }

                    DotnetRunner.Run(command, fullPath);
                }
            }
        }
    }

    private bool HasSkipProject(string projectName, ProjectContext context)
    {
        if (projectName.Contains("IntegrationTests") && !context.IncludeIntegrationTest)
            return true;

        if (projectName.Contains("ArchitectureTests") && !context.IncludeArchitectureTest)
            return true;

        return false;
    }
}
