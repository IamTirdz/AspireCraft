using AspireCraft.CLI.Commands.Projects;
using Spectre.Console.Cli;

var app = new CommandApp();

app.Configure(config =>
{
    config.AddCommand<CreateProjectCommand>("create")
        .WithDescription("Create a new project");
});

await app.RunAsync(args);
