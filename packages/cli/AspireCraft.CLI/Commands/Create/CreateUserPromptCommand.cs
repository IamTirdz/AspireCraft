using AspireCraft.CLI.Common.Extensions;
using AspireCraft.CLI.Common.Models;

namespace AspireCraft.CLI.Commands.Create;

public sealed class CreateUserPromptCommand
{
    public ProjectConfiguration PromptUser()
    {
        AppConsole.WriteLine("[yellow]Let's get started[/]", isStart: true);

        var projectName = AppConsole.Prompt("Project name");
        var architecture = AppConsole.SelectionPrompt("Architecture type", new string[] { "Clean Architecture" });
        var framework = AppConsole.SelectionPrompt("Framework", new string[] { ".NET 8.0 (LTS)", ".NET 9.0", ".NET 10.0 (preview)" });
        var useControllers = AppConsole.Prompt("Use controllers", true);
        var authentication = AppConsole.SelectionPrompt("Authentication type", new string[] { "Identity + JWT", "OpenId Connect", "None" });

        var dbContextName = AppConsole.Prompt("DbContext name:");
        var dbProvider = AppConsole.SelectionPrompt("Database Provider", new string[] { "PostgreSQL", "SQL Server", "MySQL", "SQLite" });

        // Orchestration
        var useNetAspire = AppConsole.Prompt("Enlist in .NET aspire orchestration", false);

        bool usePolly = false;
        bool useSerilog = false;
        string[] messagingOptions;
        string[] cachingOptions;
        if (useNetAspire)
        {
            usePolly = AppConsole.Prompt("Use Polly", true);
            messagingOptions = new string[] { "RabbitMQ", "Kafka", "Service Bus" };
            cachingOptions = new string[] { "Redis", "In-Memory" };
        }
        else
        {
            useSerilog = AppConsole.Prompt("Use Serilog", false);
            messagingOptions = new string[] { "RabbitMQ", "Kafka", "Service Bus", "None" };
            cachingOptions = new string[] { "Redis", "In-Memory", "None" };
        }

        var messaging = AppConsole.MultiSelectionPrompt("Messaging", messagingOptions);
        var caching = AppConsole.SelectionPrompt("Cache", cachingOptions);

        string? backgroundProcess = null;
        var includeBgProcess = AppConsole.Prompt("Include Background process?", false);
        if (includeBgProcess)
        {
            backgroundProcess = AppConsole.SelectionPrompt("Background processing", new string[] { "Hosted Services", "Quartz.NET", "Hangfire" });
        }

        List<string> testing = new List<string>();
        var includeTesting = AppConsole.Prompt("Include Tests?", false);
        if (includeTesting)
        {
            testing = AppConsole.MultiSelectionPrompt("Testing", new string[] { "Unit Tests", "Integration Tests", "Architecture Tests" });
        }

        var solutionName = projectName.Replace(" ", string.Empty);
        return new ProjectConfiguration
        {
            ProjectName = solutionName,
            Architecture = architecture,
            Framework = framework,
            AuthenticationType = authentication,
            UseControllers = useControllers,
            DatabaseContext = dbContextName,
            DatabaseProvider = dbProvider,
            UseNetAspire = useNetAspire,
            UsePolly = usePolly,
            UseSerilog = useSerilog,
            Cache = caching,
            Messaging = messaging,
            BackgroundProcess = backgroundProcess,
            Testing = testing,
        };
    }
}
