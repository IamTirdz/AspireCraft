using AspireCraft.CLI.Common.Models;
using Spectre.Console;

namespace AspireCraft.CLI.Commands;

public sealed class CreateUserPromptCommand
{
    public ProjectConfiguration PromptUser()
    {
        AnsiConsole.MarkupLine("[bold yellow]Steps[/]");
        AnsiConsole.MarkupLine("Scaffolding Category");

        var projectName = AnsiConsole.Ask<string>("Enter [green]project name[/]:");

        var projectType = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Select [green]project type[/]:")
                .AddChoices(new[] { "Clean Architecture" })
        );

        var framework = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Select [green].NET[/] version")
                .AddChoices("net 8 (LTS)", "net 9", "net 10 (preview)"));

        var features = AnsiConsole.Prompt(
            new MultiSelectionPrompt<string>()
                .Title("Select [green]features[/] to include:")
                .AddChoices(new[] { "API", "Blazor", "Identity", "MVC", "Razor Pages", "Aspire/Custom DI" })
        );

        var database = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Select a [green]database[/]:")
                .AddChoices(new[] { "PostgreSQL", "SQL Server", "MySQL", "SQLite" })
        );

        var auth = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Authentication")
                .AddChoices("Identity + JWT", "OIDC", "None")
        );

        var redis = AnsiConsole.Confirm("Include Redis?");

        return new ProjectConfiguration(projectName, framework, database, auth, redis);
    }
}
