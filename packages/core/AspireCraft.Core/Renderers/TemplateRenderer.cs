using AspireCraft.Core.Base;
using AspireCraft.Core.Common.Enums;
using AspireCraft.Core.Common.Extensions;
using AspireCraft.Core.Common.Models;

namespace AspireCraft.Core.Renderers;

public sealed class TemplateRenderer
{
    public void Run(ProjectConfiguration project)
    {
        var context = new TemplateContext
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
