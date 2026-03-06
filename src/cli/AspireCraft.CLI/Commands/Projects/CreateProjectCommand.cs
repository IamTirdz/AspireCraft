using AspireCraft.Builder.Generators;
using AspireCraft.Builder.Services;
using AspireCraft.CLI.Prompts;
using AspireCraft.Core.Models;
using Spectre.Console;
using Spectre.Console.Cli;
using System.Reflection;

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
        projectInfo.ProjectName = projectInfo.ProjectName.Replace(" ", "") + ".Backend";

        AnsiConsole.MarkupLine("[grey]│[/] ");
        AnsiConsole.MarkupLine("[grey]│[/] [green]Project initialized[/]");

        var templateGenerator = new ProjectGenerationService();
        var solutionGenerator = new SolutionGenerator();
        var projectGenerator = new ProjectGenerator();
        var referenceGenerator = new ReferenceGenerator();

        TemplateDefinition template = null!;

        AnsiConsole.Progress()
            .AutoClear(false)
            .Columns(
                new TaskDescriptionColumn(),
                new ProgressBarColumn(),
                new PercentageColumn(),
                new ElapsedTimeColumn()
            )
            .Start(ctx =>
            {
                var templateGeneratorTask = ctx.AddTask("[grey]│[/] [green]Generating template[/]");
                template = templateGenerator.Generate(projectInfo);
                templateGeneratorTask.Increment(100);

                var solutionGeneratorTask = ctx.AddTask("[grey]│[/] [green]Creating solution[/]");
                projectInfo.SolutionPath = solutionGenerator.Create(projectInfo.ProjectName, GetOutputPath());
                solutionGeneratorTask.Increment(100);

                var projectGeneratorTask = ctx.AddTask("[grey]│[/] [green]Generating project[/]");
                projectGenerator.Generate(projectInfo, template);
                projectGeneratorTask.Increment(100);

                var referenceGeneratorTask = ctx.AddTask("[grey]│[/] [green]Applying references[/]");
                referenceGenerator.ApplyReferences(projectInfo, template);
                referenceGeneratorTask.Increment(100);
            });

        AnsiConsole.MarkupLine("[grey]│[/] ");
        AnsiConsole.MarkupLine("[grey]└[/] [yellow]Project generated successfully![/]");

        ShowMigrationBanner(projectInfo.ProjectName);
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
        table.AddRow("[cyan2]Backend as Code Generator[/]", $"[grey]ver {GetAppVersion()}[/]");

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

    private string GetAppVersion()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var versionAttr = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();

        return versionAttr?.InformationalVersion?.Split('+')[0] ?? "1.0.0";
    }
}
