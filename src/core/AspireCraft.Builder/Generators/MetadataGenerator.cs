using AspireCraft.Builder.Renderers;
using AspireCraft.Core.Models;

namespace AspireCraft.Builder.Generators;

public sealed class MetadataGenerator
{

    public void Generate(ProjectContext context, TemplateDefinition template)
    {
        var featureName = "User";
        var root = Path.GetDirectoryName(context.SolutionPath)!;

        foreach (var file in template.Metadata)
        {
            var pathParts = file.Path.Replace("{{feature}}", $"{featureName}").Split('/');
            var projectShortName = pathParts[0];

            if (!context.ProjectPath.TryGetValue(projectShortName, out var projectPhysicalPath))
            {
                projectPhysicalPath = Path.Combine(root, "src", $"{context.ProjectName}.{projectShortName}");
            }

            var internalPath = string.Join(Path.DirectorySeparatorChar, pathParts.Skip(1));
            var folder = Path.Combine(projectPhysicalPath, internalPath);

            var className = file.Name.Replace("{{feature}}", featureName);
            var namespaceName = $"{context.ProjectName}.{file.Path.Replace("/", ".")}".Replace(".{{feature}}", $".{featureName}");

            var model = new
            {
                name = className,
                @namespace = namespaceName,
                feature = featureName,
                type = file.Type,
                usings = GetInheritance(file.Type, context.ProjectName)
            };

            var templatePath = Path.Combine("templates", file.Template);
            var templateRenderer = new TemplateRenderer();
            var result = templateRenderer.Render(templatePath, model);

            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

            var outputFile = Path.Combine(folder, $"{className}.cs");
            File.WriteAllText(outputFile, result);
        }
    }

    private static List<string> GetInheritance(string type, string projectName)
    {
        var list = new List<string> { "System", "System.Collections.Generic", "System.Linq", "System.Text", "System.Threading.Tasks" };

        switch (type)
        {
            case "controller":
                list.Add("Microsoft.AspNetCore.Mvc");
                list.Add("MediatR");
                list.Add("System.Net");
                list.Add("Asp.Versioning");
                break;
            case "middleware":
                list.Add($"{projectName}.Application.Common.Exceptions");
                list.Add($"{projectName}.Application.Common.Models");
                list.Add("System.Diagnostics");
                list.Add("System.Net");
                list.Add("System.Text.Json");
                break;
            case "command":
            case "query":
                list.Add("MediatR");
                break;
            case "repository":
                list.Add($"{projectName}.Domain.Interfaces");
                list.Add($"{projectName}.Domain.Entities");
                list.Add($"{projectName}.Infrastructure.Persistence.Contexts");
                list.Add("Microsoft.EntityFrameworkCore");
                break;
            case "dbcontext":
                list.Add($"{projectName}.Domain.Interfaces");
                list.Add($"{projectName}.Domain.Entities");
                list.Add("Microsoft.EntityFrameworkCore");
                break;
            case "profile":
                list.Add($"{projectName}.Domain.Entities");
                list.Add("AutoMapper");
                break;
            case "pipeline":
                list.Add($"{projectName}.Application.Common.Models");
                list.Add("MediatR");
                list.Add("FluentValidation");
                list.Add("System.Diagnostics");
                list.Add("Microsoft.Extensions.Logging");
                list.Add("System.Text.Json");
                break;
        }
        return list;
    }
}
