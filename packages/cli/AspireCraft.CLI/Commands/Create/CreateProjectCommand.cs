using AspireCraft.Core.Common.Extensions;
using AspireCraft.Core.Renderers;
using Spectre.Console;
using Spectre.Console.Cli;

namespace AspireCraft.CLI.Commands.Create;

public sealed class CreateProjectCommand : Command
{
    public override int Execute(CommandContext context, CancellationToken cancellationToken)
    {
        AppConsole.WriteLine("[yellow]Let's get started[/]", isStart: true);

        var command = new CreateUserPromptCommand();
        var configuration = command.PromptUser();

        AppConsole.WriteLine();

        TemplateEngine.CreateDefault()
            .Generate(configuration);

        AnsiConsole.WriteLine();
        AnsiConsole.WriteLine();
        var panel = new Panel(
                new Rows(
                    new Markup($"► Don't forget to migrate the database with [cyan]aspirecraft migrate[/]"),
                    new Markup($"► Then run the project")
                )
            )
            .Header($"[yellow] {configuration.ProjectName} [/]")
            .Border(BoxBorder.Rounded)
            .Padding(3, 1, 3, 1);
        AnsiConsole.Write(panel);
        AnsiConsole.WriteLine();

        return 0;
    }
}
