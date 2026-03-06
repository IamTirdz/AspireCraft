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
        var root = Path.GetDirectoryName(context.SolutionPath)!;
        var src = Path.Combine(root, template.SrcFolder ?? "src");

        Directory.CreateDirectory(src);

        var framework = Enum.GetValues<TargetFramework>()
            .Select(f => typeof(TargetFramework).GetField(f.ToString())?
            .GetCustomAttribute<DisplayAttribute>())
            .FirstOrDefault(a => a?.Name == context.Framework)?.ShortName ?? "net8.0";

        foreach (var layer in template.Layers)
        {
            CreateProject(context.ProjectName, layer, src, framework);
        }
    }

    private static void CreateProject(string solution, LayerDefinition layer, string src, string framework)
    {
        var name = $"{solution}.{layer.Name}";

        if (layer.Type == "webapi")
        {
            DotnetRunner.Run($"new webapi -n {name} -f {framework}", src);
        }
        else
        {
            DotnetRunner.Run($"new classlib -n {name} -f {framework}", src);
        }
    }
}
