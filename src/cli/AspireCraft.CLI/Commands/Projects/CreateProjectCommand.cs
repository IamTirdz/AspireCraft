using AspireCraft.Builder.Generators;
using AspireCraft.Builder.Renderers;
using AspireCraft.CLI.Prompts;
using AspireCraft.Core.Extensions;
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
        projectInfo.ProjectName = projectInfo.ProjectName.TrimAll() + ".Backend";

        var templateLoader = new TemplateLoader();
        var templatePath = Path.Combine(AppContext.BaseDirectory, "templates", "architectures", $"{projectInfo.Template.TrimAll()}.json");
        var template = templateLoader.Load(templatePath);

        var solutionGenerator = new SolutionGenerator();
        var projectGenerator = new ProjectGenerator();
        var referenceGenerator = new ReferenceGenerator();
        var templateGenerator = new TemplateGenerator();

        var table = new Table()
            .NoBorder()
            .HideHeaders()
            .AddColumn(new TableColumn("Icon").Padding(0, 0, 0, 0))
            .AddColumn(new TableColumn("Status").Padding(0, 0, 0, 0));

        AnsiConsole.MarkupLine("[grey]│[/] ");
        AnsiConsole.Live(table)
            .Start(ctx =>
            {
                void SetInProgress(int row, string label)
                {
                    table.UpdateCell(row, 1, $"[grey]{label} - In-Progress[/]");
                    ctx.Refresh();
                }

                void SetCompleted(int row, string label)
                {
                    table.UpdateCell(row, 1, $"[green]{label} - Completed[/]");
                    ctx.Refresh();
                }

                var iconLabel = "[grey]│[/]";
                var templateTask = "Generating template";
                var solutiontask = "Creating solution";
                var projectTask = "Generating project";
                var referenceTask = "Applying reference";

                table.AddRow(iconLabel, templateTask);
                table.AddRow(iconLabel, solutiontask);
                table.AddRow(iconLabel, projectTask);
                table.AddRow(iconLabel, referenceTask);
                ctx.Refresh();

                SetInProgress(0, solutiontask);
                projectInfo.SolutionPath = solutionGenerator.Create(projectInfo.ProjectName, GetOutputPath());
                SetCompleted(0, solutiontask);

                SetInProgress(1, projectTask);
                projectGenerator.Generate(projectInfo, template);
                SetCompleted(1, projectTask);

                SetInProgress(2, referenceTask);
                referenceGenerator.ApplyReferences(projectInfo, template);
                SetCompleted(2, referenceTask);

                SetInProgress(3, templateTask);
                templateGenerator.Generate(projectInfo);
                SetCompleted(3, templateTask);
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
