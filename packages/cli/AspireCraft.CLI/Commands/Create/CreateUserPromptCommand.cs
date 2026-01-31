using AspireCraft.Core.Base;
using AspireCraft.Core.Common.Enums;
using AspireCraft.Core.Common.Extensions;
using AspireCraft.Core.Common.Models;

namespace AspireCraft.CLI.Commands.Create;

public sealed class CreateUserPromptCommand
{
    public ProjectConfiguration PromptUser()
    {
        // Project Info
        var projectName = Prompt.Ask("Project name:");
        var architecture = Prompt.Select<ArchitectureType>("Architecture type:");
        var framework = Prompt.Select<NetFramework>(".NET Framework:");

        bool? useControllers = null;
        AuthenticationType? authentication = null;
        if (architecture != ArchitectureType.Serverless)
        {
            useControllers = Prompt.Confirm("Use controllers?", true);
            authentication = Prompt.Select<AuthenticationType>("Authentication type:");
        }

        var dbContextName = Prompt.Ask("DbContext name:");
        var dbProvider = Prompt.Select<DatabaseProvider>("Database Provider:");

        // Orchestration
        var useNetAspire = Prompt.Confirm("Use .NET aspire orchestration?", false);

        var availableIntegrations = IntegrationCatalog.GetAvailableIntegrations(useNetAspire);
        var selectedIntegrations = new List<IntegrationType>();

        // Integrations
        var emailIntegrations = availableIntegrations.AddIntegrations(new[] { IntegrationType.SendGrid, IntegrationType.Mailgun });
        var emailIntegration = Prompt.MultiSelect("Email", emailIntegrations, includeNone: true);
        if (emailIntegration != null)
            selectedIntegrations.AddRange(emailIntegration);

        var smsIntegrations = availableIntegrations.AddIntegrations(new[] { IntegrationType.Wavecell, IntegrationType.Twilio });
        var smsIntegration = Prompt.MultiSelect("SMS", smsIntegrations, includeNone: true);
        if (smsIntegration != null)
            selectedIntegrations.AddRange(smsIntegration);

        var storageIntegrations = availableIntegrations.AddIntegrations(new[] { IntegrationType.AzureBlob, IntegrationType.AwsS3Bucket });
        var storageIntegration = Prompt.MultiSelect("Storage", storageIntegrations, includeNone: true);
        if (storageIntegration != null)
            selectedIntegrations.AddRange(storageIntegration);

        var messagingIntegrations = availableIntegrations.AddIntegrations(new[] { IntegrationType.RabbitMQ, IntegrationType.Kafka, IntegrationType.ServiceBus });
        var messagingIntegration = Prompt.MultiSelect("Messaging", messagingIntegrations, includeNone: true);
        if (messagingIntegration != null)
            selectedIntegrations.AddRange(messagingIntegration);

        var cacheIntegrations = availableIntegrations.AddIntegrations(new[] { IntegrationType.Redis, IntegrationType.InMemory });
        var cacheIntegration = Prompt.MultiSelect("Caching", cacheIntegrations, includeNone: true);
        if (cacheIntegration != null)
            selectedIntegrations.AddRange(cacheIntegration);

        var paymentIntegrations = availableIntegrations.AddIntegrations(new[] { IntegrationType.Paypal, IntegrationType.Stripe });
        var paymentIntegration = Prompt.MultiSelect("Payment", paymentIntegrations, includeNone: true);
        if (paymentIntegration != null)
            selectedIntegrations.AddRange(paymentIntegration);

        // Test options
        var includeUnitTest = Prompt.Confirm("Include Unit tests?", false);
        var includeIntegrationTest = Prompt.Confirm("Include Integration tests?", false);
        var includeArchitectureTest = Prompt.Confirm("Include Architecture tests?", false);

        var solutionName = projectName.Replace(" ", string.Empty);
        return new ProjectConfiguration
        {
            ProjectName = solutionName,
            Architecture = architecture,
            Framework = framework,
            Authentication = authentication,
            UseControllers = useControllers.HasValue && useControllers.Value,
            DbContextName = dbContextName,
            Database = dbProvider,
            UseNetAspire = useNetAspire,
            Integrations = selectedIntegrations,
            IncludeUnitTests = includeUnitTest,
            IncludeIntegrationTests = includeIntegrationTest,
            IncludeArchitectureTests = includeArchitectureTest,
        };
    }
}
