using AspireCraft.CLI.Common.Enums;
using AspireCraft.CLI.Common.Extensions;
using AspireCraft.CLI.Common.Models;
using AspireCraft.CLI.Renderers;
using AspireCraft.Core.Base;

namespace AspireCraft.Core;

public static class ProjectGenerator
{
    public static void Run(ProjectConfiguration project)
    {
        var context = new GenerationContext
        {
            RootPath = Path.Combine(Directory.GetCurrentDirectory(), project.ProjectName)
        };

        var template = project.Architecture switch
        {
            ArchitectureType.CleanArchitecture => new CleanArchitecture(),
            ArchitectureType.NLayer => throw new NotSupportedException("NLayer was not supported yet"),
            ArchitectureType.Serverless => throw new NotSupportedException("Serverless was not supported yet"),
            _ => throw new NotSupportedException("Selected template is not supported.")
        };

        template.Generate(project, context);

        AppConsole.WriteLine($"[green] Project {project.ProjectName} generated successfully![/]", includeIndicator: false);
    }
}
