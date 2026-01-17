using AspireCraft.CLI.Renderers;
using Spectre.Console;
using Spectre.Console.Cli;

namespace AspireCraft.CLI.Commands;

public sealed class CreateProjectCommand : Command
{
    public override int Execute(CommandContext context, CancellationToken cancellationToken)
    {
        var command = new CreateUserPromptCommand();
        var config = command.PromptUser();

        AnsiConsole.MarkupLine("[green]Generating project...[/]");

        var renderer = new ProjectTemplateRenderer();
        renderer.Generate(config);

        AnsiConsole.MarkupLine("[green]Done![/]");

        return 0;
    }
}
