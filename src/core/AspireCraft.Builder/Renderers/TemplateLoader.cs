using AspireCraft.Core.Models;
using System.Text.Json;

namespace AspireCraft.Builder.Renderers;

public class TemplateLoader
{
    public TemplateDefinition Load(string path)
    {
        var json = File.ReadAllText(path);

        return JsonSerializer.Deserialize<TemplateDefinition>(json,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
    }
}
