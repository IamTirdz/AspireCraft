using AspireCraft.Builder.Generators;
using AspireCraft.CLI.Prompts;
using AspireCraft.Runtime;
using Spectre.Console;
using Spectre.Console.Cli;

namespace AspireCraft.CLI.Commands.Projects;

public sealed class CreateProjectCommand : Command<CreateProjectCommand.Settings>
{
    public class Settings : CommandSettings { }

    public override int Execute(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        ConsoleHeader();
        AnsiConsole.MarkupLine("[grey]┌[/] [yellow]Let's get started[/]");

        var prompts = new PromptBuilder()
            .AddProjectSetupPrompts()
            .AddArchitecturePrompts()
            .AddDatabasePrompts()
            .AddIntegrationPrompts()
            .AddSecurityPrompts()
            .AddTestPrompts()
            .Build();

        var wizard = new PromptWizard();
        var userInputs = wizard.Ask(prompts);
        var projectInfo = wizard.GetInputs(userInputs);

        AnsiConsole.MarkupLine("[green]Project initialized[/]");

        var engine = new ProjectGenerator();
        AnsiConsole.Progress()
            .Start(ctx =>
            {
                var task = ctx.AddTask("[green]Generating project[/]");
                engine.Generate(projectInfo);
                task.Increment(100);
            });

        var solutionManager = new SolutionManager();
        var outputPath = GetOutputPath();
        AnsiConsole.Progress()
           .Start(ctx =>
           {
               var task = ctx.AddTask("[green]Craeting solution[/]");
               solutionManager.CreateSolution(projectInfo, outputPath);
               task.Increment(100);
           });

        ShowMigrationBanner(projectInfo.Name);
        return 0;
    }

    private string GetOutputPath()
    {
#if DEBUG
        var localFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        return Path.Combine(localFolder, "source", "repos");
#else
        return Directory.GetCurrentDirectory();
#endif
    }

    private void ConsoleHeader()
    {
        AnsiConsole.Write(new FigletText("ASPIRE CRAFT").Color(Color.Cyan1));

        var table = new Table()
            .Border(TableBorder.None)
            .Expand();
        table.AddColumn(new TableColumn("").LeftAligned());
        table.AddColumn(new TableColumn("").RightAligned());
        table.AddRow("[cyan2]Backend as Code Generator[/]", "[grey]ver 1.1.0[/]");

        AnsiConsole.Write(table);
        AnsiConsole.WriteLine();
    }

    private void ShowMigrationBanner(string projectName)
    {
        AnsiConsole.WriteLine();
        var panel = new Panel(
                new Rows(
                    new Markup($"► Don't forget to migrate the database with [cyan]aspirecraft database migrate[/]"),
                    new Markup($"► Then run the project")
                )
            )
            .Header($"[yellow] [bold]{projectName}[/] [/]")
            .Border(BoxBorder.Rounded)
            .Padding(3, 1, 3, 1);

        AnsiConsole.Write(panel);
    }
}
