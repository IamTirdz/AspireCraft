using AspireCraft.Core.Extensions;
using AspireCraft.Core.Models;

namespace AspireCraft.Builder.Services;

public sealed class TemplateRenderer
{
    public string Render(string templateContent, ProjectContext context)
    {
        if (templateContent == null) throw new ArgumentNullException(nameof(templateContent));
        if (context == null) throw new ArgumentNullException(nameof(context));

        templateContent = templateContent
            .Replace("{{ProjectName}}", context.Name.TrimAll())
            .Replace("{{Template}}", context.Template)
            .Replace("{{Framework}}", context.Framework)
            .Replace("{{Database}}", context.Database)
            .Replace("{{Authentication}}", context.Authentication)
            .Replace("{{MockLibrary}}", context.MockLibrary);

        templateContent = templateContent
            .Replace("{{Modules}}", string.Join(", ", context.Modules))
            .Replace("{{TestProjects}}", string.Join(", ", context.TestProjects));

        return templateContent;
    }
}
