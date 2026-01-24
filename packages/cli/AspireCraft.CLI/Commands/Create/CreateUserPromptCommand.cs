using AspireCraft.Core.Common.Enums;
using AspireCraft.Core.Common.Extensions;
using AspireCraft.Core.Common.Models;

namespace AspireCraft.CLI.Commands.Create;

public sealed class CreateUserPromptCommand
{
    public ProjectConfiguration PromptUser()
    {
        AppConsole.WriteLine("[yellow]Let's get started[/]", isStart: true);

        var projectName = AppConsole.Prompt("Project name");
        var architecture = AppConsole.SelectionPrompt("Architecture type", Enum.GetValues(typeof(ArchitectureType)).ToEnumList());
        var framework = AppConsole.SelectionPrompt("Framework", Enum.GetValues(typeof(NetFramework)).ToEnumList());
        var useControllers = AppConsole.Prompt("Use controllers?", true);
        var authentication = AppConsole.SelectionPrompt("Authentication type", Enum.GetValues(typeof(AuthenticationType)).ToEnumList());

        var dbContextName = AppConsole.Prompt("DbContext name:");
        var dbProvider = AppConsole.SelectionPrompt("Database Provider", Enum.GetValues(typeof(DatabaseProvider)).ToEnumList());

        var useNetAspire = AppConsole.Prompt("Use .NET aspire orchestration?", false);
        var availableIntegrations = IntegrationCatalog.GetAvailableIntegrations(useNetAspire);

        var selectedIntegrations = new List<IntegrationType>();

        var emailIntegrations = availableIntegrations
            .Where(i => i == IntegrationType.SendGrid || i == IntegrationType.Mailgun)
            .Select(i => i.ToEnumValue())
            .ToList();
        var emailIntegration = AppConsole.MultiSelectionPrompt("Email", emailIntegrations);
        selectedIntegrations.AddRange(emailIntegration.Select(name => Enum.Parse<IntegrationType>(name)));

        var smsIntegrations = availableIntegrations
            .Where(i => i == IntegrationType.Wavecell || i == IntegrationType.Twilio)
            .Select(i => i.ToEnumValue())
            .ToList();
        var smsIntegration = AppConsole.MultiSelectionPrompt("SMS", smsIntegrations);
        selectedIntegrations.AddRange(smsIntegration.Select(name => Enum.Parse<IntegrationType>(name)));

        var storageIntegrations = availableIntegrations
            .Where(i => i == IntegrationType.AzureBlob || i == IntegrationType.AwsS3Bucket)
            .Select(i => i.ToEnumValue())
            .ToList();
        var storageIntegration = AppConsole.MultiSelectionPrompt("Storage", storageIntegrations);
        selectedIntegrations.AddRange(storageIntegration.Select(name => Enum.Parse<IntegrationType>(name)));

        var messagingIntegrations = availableIntegrations
            .Where(i => i == IntegrationType.RabbitMQ || i == IntegrationType.Kafka || i == IntegrationType.ServiceBus)
            .Select(i => i.ToEnumValue())
            .ToList();
        var messagingIntegration = AppConsole.MultiSelectionPrompt("Messaging", messagingIntegrations);
        selectedIntegrations.AddRange(messagingIntegration.Select(name => Enum.Parse<IntegrationType>(name)));

        var cacheIntegrations = availableIntegrations
            .Where(i => i == IntegrationType.Redis || i == IntegrationType.InMemory)
            .Select(i => i.ToEnumValue())
            .ToList();
        var cacheIntegration = AppConsole.MultiSelectionPrompt("Caching", cacheIntegrations);
        selectedIntegrations.AddRange(cacheIntegration.Select(name => Enum.Parse<IntegrationType>(name)));

        var paymentIntegrations = availableIntegrations
            .Where(i => i == IntegrationType.Paypal || i == IntegrationType.Stripe)
            .Select(i => i.ToEnumValue())
            .ToList();
        var paymentIntegration = AppConsole.MultiSelectionPrompt("Payment", paymentIntegrations);
        selectedIntegrations.AddRange(paymentIntegration.Select(name => Enum.Parse<IntegrationType>(name)));

        var includeUnitTest = AppConsole.Prompt("Include Unit Tests?", false);
        var includeIntegrationTest = AppConsole.Prompt("Include Integration Tests?", false);
        var includeArchitectureTest = AppConsole.Prompt("Include Architecture Tests?", false);

        var solutionName = projectName.Replace(" ", string.Empty);
        return new ProjectConfiguration
        {
            ProjectName = solutionName,
            Architecture = (ArchitectureType)Enum.Parse(typeof(ArchitectureType), architecture),
            Framework = framework,
            Authentication = (AuthenticationType)Enum.Parse(typeof(AuthenticationType), authentication),
            UseControllers = useControllers,
            DbContextName = dbContextName,
            Database = (DatabaseProvider)Enum.Parse(typeof(DatabaseProvider), dbProvider),
            UseNetAspire = useNetAspire,
            Integrations = selectedIntegrations,
            IncludeUnitTests = includeUnitTest,
            IncludeIntegrationTests = includeIntegrationTest,
            IncludeArchitectureTests = includeArchitectureTest,
        };
    }
}
