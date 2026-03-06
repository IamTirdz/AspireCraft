using AspireCraft.Core.Abstractions;
using AspireCraft.Core.Models;

namespace AspireCraft.Plugins.Templates;

public sealed class CleanArchitecturePlugin : ITemplatePlugin
{
    public string Name => "Clean Architecture";

    public TemplateDefinition GetDefinition()
    {
        return new TemplateDefinition
        {
            Name = "CleanArchitecture",

            Layers =
            [
                new() { Name = "API", Type = "webapi" },
                new() { Name = "Application", Type = "classlib" },
                new() { Name = "Domain", Type = "classlib" },
                new() { Name = "Infrastructure", Type = "classlib" }
            ],

            References =
            [
                new() { From = "API", To = "Application" },
                new() { From = "Infrastructure", To = "Application" },
                new() { From = "Application", To = "Domain" }
            ],

            Tests =
            [
                new() { Name = "UnitTests", Target = "Application" },
                new() { Name = "IntegrationTests", Target = "API" },
                new() { Name = "ArchitectureTests", Target = "Domain" },
            ],
        };
    }
}
