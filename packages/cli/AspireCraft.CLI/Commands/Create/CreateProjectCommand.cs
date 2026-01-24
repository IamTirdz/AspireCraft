using AspireCraft.Core.Common.Extensions;
using AspireCraft.Core.Renderers;
using Spectre.Console;
using Spectre.Console.Cli;

namespace AspireCraft.CLI.Commands.Create;

public sealed class CreateProjectCommand : Command
{
    public override int Execute(CommandContext context, CancellationToken cancellationToken)
    {
        var command = new CreateUserPromptCommand();
        var config = command.PromptUser();

        AppConsole.WriteLine();

        var renderer = new TemplateRenderer();
        renderer.Run(config);

        AnsiConsole.WriteLine();
        AnsiConsole.WriteLine();
        var panel = new Panel(
                new Rows(
                    new Markup($"► Don't forget to migrate the database with [cyan]aspirecraft migrate[/]"),
                    new Markup($"► Then run the project")
                )
            )
            .Header($"[yellow] {config.ProjectName} [/]")
            .Border(BoxBorder.Rounded)
            .Padding(3, 1, 3, 1);
        AnsiConsole.Write(panel);
        AnsiConsole.WriteLine();

        return 0;
    }
}
