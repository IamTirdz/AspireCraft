using AspireCraft.Builder.Renderers;
using AspireCraft.Core.Models;

namespace AspireCraft.Builder.Generators;

public sealed class MetadataGenerator
{
    public void Generate(ProjectContext context, TemplateDefinition template)
    {
        var features = new List<string> { "Product", "Order" };
        var root = Path.GetDirectoryName(context.SolutionPath)!;

        foreach (var file in template.Metadata)
        {
            bool isFeature = file.Name.Contains("{{feature}}") || file.Path.Contains("{{feature}}");
            if (isFeature)
            {
                foreach (var feature in features)
                {
                    RenderFile(file, context, root, feature);
                }
            }
            else
            {
                RenderFile(file, context, root, "");
            }
        }
    }

    private void RenderFile(MetadataDefinition file, ProjectContext context, string root, string feature)
    {
        var currentFeature = feature ?? string.Empty;
        var targetFolder = ResolvePath(file, context, root, currentFeature);
        var className = file.Name.Replace("{{feature}}", currentFeature);
        var namespaceName = $"{context.ProjectName}.{file.Path.Replace("/", ".").Replace("{{feature}}", currentFeature)}";
        var hostName = context.ProjectName.Replace(".", "_");
        var projectName = context.ProjectName.Replace(".", " ");

        var model = new
        {
            name = className,
            @namespace = namespaceName,
            hostname = hostName,
            projectname = projectName,
            feature = currentFeature,
            variant = file.Variant,
            @base = file.Base ?? string.Empty,
            usings = GetInheritance(file.Type, file.Variant, currentFeature, context.ProjectName),
            database = context.Database.ToLower(),
        };

        var templatePath = Path.Combine("templates", file.Template);
        var result = new TemplateRenderer().Render(templatePath, model);

        if (!Directory.Exists(targetFolder))
        {
            Directory.CreateDirectory(targetFolder!);
        }

        var outputFile = Path.Combine(targetFolder!, $"{className}.cs");
        File.WriteAllText(outputFile, result);
    }

    private static string? ResolvePath(MetadataDefinition file, ProjectContext context, string root, string featureName)
    {
        var pathParts = file.Path.Replace("{{feature}}", featureName).Split('/');
        var projectShortName = pathParts[0];

        if (!context.ProjectPath.TryGetValue(projectShortName, out var projectPhysicalPath))
        {
            // Fallback: convention-based path
            projectPhysicalPath = Path.Combine(root, "src", $"{context.ProjectName}.{projectShortName}");
        }

        var internalPath = string.Join(Path.DirectorySeparatorChar, pathParts.Skip(1));
        return Path.Combine(projectPhysicalPath, internalPath);
    }

    private static List<string> GetInheritance(string type, string variant, string feature, string projectName)
    {
        var list = new List<string>();

        var @using = type switch
        {
            "controller" =>
                variant switch
                {
                    "base" =>
                        new[]
                        {
                            "MediatR",
                            "Asp.Versioning",
                            "Microsoft.AspNetCore.Mvc",
                        },
                    "controller" =>
                        new[]
                        {
                            "Asp.Versioning",
                            "Microsoft.AspNetCore.Mvc",
                            $"{projectName}.Application.{feature}s.Queries.Get{feature}s",
                            $"{projectName}.Application.{feature}s.Commands.Create{feature}",
                            $"{projectName}.Application.{feature}s.Queries.Get{feature}ById"
                        },
                    _ => Array.Empty<string>()
                },
            "middleware" =>
                new[]
                {
                    $"{projectName}.Application.Common.Exceptions",
                    $"{projectName}.Application.Common.Models",
                    "System.Diagnostics",
                    "System.Net",
                    "System.Text.Json",
                },
            //"entity" =>
            "repository" =>
                variant switch
                {
                    "interface" =>
                        new[]
                        {
                            $"{projectName}.Domain.Entities",
                        },
                    "base" =>
                        new[]
                        {
                            $"{projectName}.Domain.Interfaces",
                            $"{projectName}.Domain.Entities",
                            "Microsoft.EntityFrameworkCore",
                        },
                    "implementation" =>
                        new[]
                        {
                            $"{projectName}.Domain.Interfaces",
                            $"{projectName}.Domain.Entities",
                        },
                    _ => Array.Empty<string>()
                },
            "dbcontext" =>
                new[]
                {
                    $"{projectName}.Domain.Interfaces",
                    $"{projectName}.Domain.Entities",
                    "Microsoft.EntityFrameworkCore",
                    "Microsoft.AspNetCore.Identity.EntityFrameworkCore",
                    "Microsoft.AspNetCore.Identity",
                    "System.Reflection",
                    $"{projectName}.Infrastructure.Identity",
                },
            "class" =>
                variant switch
                {
                    "command" or
                    "queries" =>
                        new[]
                        {
                            "MediatR",
                        },
                    "query" =>
                        new[]
                        {
                            "MediatR",
                             $"{projectName}.Application.{feature}s.Queries.Get{feature}s"
                        },
                    "profile" =>
                        new[]
                        {
                            "AutoMapper",
                            $"{projectName}.Domain.Entities",
                            $"{projectName}.Application.{feature}s.Queries.Get{feature}s"
                        },
                    _ => Array.Empty<string>()
                },
            "pipeline" =>
                 new[] {
                    $"{projectName}.Application.Common.Models",
                    "MediatR",
                    "FluentValidation",
                    "System.Diagnostics",
                    "Microsoft.Extensions.Logging",
                    "System.Text.Json",
                 },
            "model" =>
                variant switch
                {
                    "user-identity" or
                    "role-identity" =>
                        new[]
                        {
                            "Microsoft.AspNetCore.Identity",
                        },
                    _ => Array.Empty<string>()
                },
            "exception" =>
                 new[] {
                    $"{projectName}.Application.Common.Models",
                 },
            "program" =>
                variant switch
                {
                    "host" =>
                        new[]
                        {
                            "Microsoft.Extensions.Configuration",
                            "Microsoft.Extensions.DependencyInjection",
                            "Microsoft.Extensions.Hosting"
                        },
                    "api" =>
                        new[]
                        {
                            "Microsoft.Extensions.Hosting",
                            $"{projectName}.API.Middlewares",
                            $"{projectName}.API",
                            $"{projectName}.Application",
                            $"{projectName}.Infrastructure"
                        },
                    _ => Array.Empty<string>()
                },
            "dependency-injection" =>
                variant switch
                {
                    "api" =>
                        new[]
                        {
                            "Asp.Versioning",
                            "Microsoft.AspNetCore.HttpLogging",
                            "Microsoft.AspNetCore.Mvc",
                            "Microsoft.AspNetCore.ResponseCompression",
                            "NSwag",
                            "NSwag.Generation.Processors.Security",
                            $"{projectName}.API.Middlewares"
                        },
                    "application" =>
                        new[]
                        {
                            "AutoMapper",
                            "System.Reflection",
                            $"{projectName}.Domain.Entities",
                            "Microsoft.Extensions.Hosting",
                            "FluentValidation",
                            "Microsoft.Extensions.DependencyInjection",
                            "MediatR",
                            $"{projectName}.Application.Common.Behaviors"
                        },
                    "infrastructure" =>
                        new[]
                        {
                            $"{projectName}.Infrastructure.Persistence",
                            "Microsoft.Extensions.Hosting",
                            "Microsoft.Extensions.DependencyInjection"
                        },
                    _ => Array.Empty<string>()
                },
            _ => Array.Empty<string>()
        };

        list.AddRange(@using);
        return list.Distinct().ToList();
    }
}
