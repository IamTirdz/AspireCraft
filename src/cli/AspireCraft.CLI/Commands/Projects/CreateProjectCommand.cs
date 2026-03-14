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
        try
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
            var templatePath = Path.Combine(AppContext.BaseDirectory, "templates", "architecture", $"{projectInfo.Template.TrimAll()}.json");
            var template = templateLoader.Load(templatePath);

            var solutionGenerator = new SolutionGenerator();
            var projectGenerator = new ProjectGenerator();
            var referenceGenerator = new ReferenceGenerator();
            var packageGenerator = new PackageGenerator();
            var templateGenerator = new MetadataGenerator();

            var table = new Table()
                .NoBorder()
                .HideHeaders()
                .AddColumn(new TableColumn("Icon").Padding(0, 0, 0, 0))
                .AddColumn(new TableColumn("Status").Padding(0, 0, 0, 0));

            var label = "[grey]│[/] ";
            var currentStep = 0;
            var steps = new List<string>
            {
                "Creating solution",
                "Generating projects",
                "Applying references",
                "Adding packages",
                 "Building template",
            };

            foreach (var step in steps)
            {
                table.AddRow(label, step);
            }

            AnsiConsole.MarkupLine("[grey]│[/] ");
            AnsiConsole.Live(table)
                .Start(ctx =>
                {
                    int GetIndex(string label) => steps.IndexOf(label);

                    void SetStatus(string label, string icon, string color, string status)
                    {
                        int row = GetIndex(label);
                        table.UpdateCell(row, 0, $"[{color}]{Markup.Escape(icon)}[/]");
                        table.UpdateCell(row, 1, $"[{color}]{Markup.Escape(label)} - {Markup.Escape(status)}[/]");
                        ctx.Refresh();
                    }

                    try
                    {
                        currentStep = GetIndex("Creating solution");
                        SetStatus(steps[currentStep], "○", "yellow", "Running...");
                        projectInfo.SolutionPath = solutionGenerator.Create(projectInfo.ProjectName, GetOutputPath());
                        SetStatus(steps[currentStep], "✔", "green", "Done");

                        currentStep = GetIndex("Generating projects");
                        SetStatus(steps[currentStep], "○", "yellow", "Running...");
                        projectGenerator.Generate(projectInfo, template);
                        SetStatus(steps[currentStep], "✔", "green", "Done");

                        currentStep = GetIndex("Applying references");
                        SetStatus(steps[currentStep], "○", "yellow", "Running...");
                        referenceGenerator.ApplyReferences(projectInfo, template);
                        SetStatus(steps[currentStep], "✔", "green", "Done");

                        currentStep = GetIndex("Adding packages");
                        SetStatus(steps[currentStep], "○", "yellow", "Installing...");
                        packageGenerator.Generate(projectInfo, template);
                        SetStatus(steps[currentStep], "✔", "green", "Done");

                        currentStep = GetIndex("Building template");
                        SetStatus(steps[currentStep], "○", "yellow", "Generating...");
                        templateGenerator.Generate(projectInfo, template);
                        SetStatus(steps[currentStep], "✔", "green", "Done");
                    }
                    catch (Exception ex)
                    {
                        SetStatus(steps[currentStep], "✘", "red", $"Failed: {ex.Message}");
                        throw;
                    }
                });

            AnsiConsole.MarkupLine("[grey]│[/] ");
            AnsiConsole.MarkupLine("[grey]└[/] [yellow]Project generated successfully![/]");

            ShowMigrationBanner(projectInfo.ProjectName);
        }
        catch (Exception ex)
        {
            ShowFailureBanner(ex.Message);
        }

        return 0;
    }

    private static string GetOutputPath()
    {
#if DEBUG
        var localFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        return Path.Combine(localFolder, "source", "repos");
#else
        return Directory.GetCurrentDirectory();
#endif
    }

    private static void ConsoleHeader()
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

    private static void ShowMigrationBanner(string projectName)
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

    private static void ShowFailureBanner(string errorMessage)
    {
        AnsiConsole.WriteLine();

        var panel = new Panel(
                new Rows(
                    new Markup($"[red]► Error:[/]"),
                    new Markup($"[dim]► Message:[/] {Markup.Escape(errorMessage)}"),
                    new Markup($"[yellow]► Please check logs or try 'dotnet clean' and retry.[/]")
                )
            )
            .Header("[red] [bold]OPERATION FAILED[/] [/]")
            .Border(BoxBorder.Rounded)
            .BorderColor(Color.Red)
            .Padding(3, 1, 3, 1);

        AnsiConsole.Write(panel);
    }

    private static string GetAppVersion()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var versionAttr = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();

        return versionAttr?.InformationalVersion?.Split('+')[0] ?? "1.0.0";
    }
}
