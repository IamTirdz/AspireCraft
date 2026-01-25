using AspireCraft.CLI.Commands.Create;
using Spectre.Console;
using Spectre.Console.Cli;
using System.Text;

var app = new CommandApp();

Console.OutputEncoding = Encoding.UTF8;
AnsiConsole.Write(new FigletText("ASPIRE CRAFT").Color(Color.Cyan1));

var leftText = "[cyan2]Backend as Code Generator[/]";
var rightText = "[grey]ver 1.1.0[/]";
var width = Console.WindowWidth;
var header = leftText + new string(' ', Math.Max(1, width - leftText.Length - rightText.Length)) + rightText;
AnsiConsole.MarkupLine(header);

app.Configure(config =>
{
    config.AddCommand<CreateProjectCommand>("create")
        .WithDescription("Create new project");
});

await app.RunAsync(args);
