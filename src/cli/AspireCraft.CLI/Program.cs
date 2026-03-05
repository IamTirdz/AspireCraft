using AspireCraft.CLI.Commands.Projects;
using Spectre.Console.Cli;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;

var app = new CommandApp();

app.Configure(config =>
{
    config.AddCommand<CreateProjectCommand>("create")
        .WithDescription("Create a new project");
});

await app.RunAsync(args);
