using AspireCraft.Builder.Renderers;
using AspireCraft.Core.Models;

namespace AspireCraft.Builder.Generators;

public sealed class TemplateGenerator
{
    public void Generate(ProjectContext context)
    {
        var templateRoot = Path.Combine(AppContext.BaseDirectory, "templates", "modules");
        var modules = Directory.GetFiles(templateRoot, "*.scriban", SearchOption.AllDirectories);

        foreach (var module in modules)
        {
            var relative = Path.GetRelativePath(templateRoot, module)
                .Replace(".scriban", string.Empty);

            var parts = relative.Split(Path.DirectorySeparatorChar);
            var project = parts[0];
            if (!context.ProjectPath.ContainsKey(project))
                continue;

            var projectPath = context.ProjectPath[project];
            var targetRelative = Path.Combine(parts.Skip(1).ToArray())
                .Replace(".scriban", string.Empty);
            var targetPath = Path.Combine(projectPath, targetRelative);

            Directory.CreateDirectory(Path.GetDirectoryName(targetPath)!);

            var content = TemplateRenderer.Render(module, new
            {
                ProjectName = context.ProjectName,
                Namespace = $"{context.ProjectName}.{project}"
            });

            File.WriteAllText(targetPath, content);
        }
    }
}
