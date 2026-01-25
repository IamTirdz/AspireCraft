using AspireCraft.Core.Base;
using AspireCraft.Core.Common.Constants;
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

        bool? useControllers = null;
        string? authentication = null;
        if (architecture != ArchitectureType.Serverless.ToString())
        {
            useControllers = AppConsole.Prompt("Use controllers?", true);
            authentication = AppConsole.SelectionPrompt("Authentication type", Enum.GetValues(typeof(AuthenticationType)).ToEnumList());
        }

        var dbContextName = AppConsole.Prompt("DbContext name:");
        var dbProvider = AppConsole.SelectionPrompt("Database Provider", Enum.GetValues(typeof(DatabaseProvider)).ToEnumList());

        var useNetAspire = AppConsole.Prompt("Use .NET aspire orchestration?", false);
        var availableIntegrations = IntegrationCatalog.GetAvailableIntegrations(useNetAspire);

        var selectedIntegrations = new List<IntegrationType>();

        var emailIntegrations = availableIntegrations
            .Where(i => i == IntegrationType.SendGrid || i == IntegrationType.Mailgun)
            .Select(i => i.ToEnumValue())
            .ToList();
        emailIntegrations.Insert(0, AppConstant.NoneOption);
        var emailIntegration = AppConsole.MultiSelectionPrompt("Email", emailIntegrations);
        if (!emailIntegration.Contains(AppConstant.NoneOption))
            selectedIntegrations.AddRange(emailIntegration.Select(e => e.FromEnumValue<IntegrationType>()));

        var smsIntegrations = availableIntegrations
            .Where(i => i == IntegrationType.Wavecell || i == IntegrationType.Twilio)
            .Select(i => i.ToEnumValue())
            .ToList();
        smsIntegrations.Insert(0, AppConstant.NoneOption);
        var smsIntegration = AppConsole.MultiSelectionPrompt("SMS", smsIntegrations);
        if (!smsIntegration.Contains(AppConstant.NoneOption))
            selectedIntegrations.AddRange(smsIntegration.Select(s => s.FromEnumValue<IntegrationType>()));

        var storageIntegrations = availableIntegrations
            .Where(i => i == IntegrationType.AzureBlob || i == IntegrationType.AwsS3Bucket)
            .Select(i => i.ToEnumValue())
            .ToList();
        storageIntegrations.Insert(0, AppConstant.NoneOption);
        var storageIntegration = AppConsole.MultiSelectionPrompt("Storage", storageIntegrations);
        if (!storageIntegration.Contains(AppConstant.NoneOption))
            selectedIntegrations.AddRange(storageIntegration.Select(s => s.FromEnumValue<IntegrationType>()));

        var messagingIntegrations = availableIntegrations
            .Where(i => i == IntegrationType.RabbitMQ || i == IntegrationType.Kafka || i == IntegrationType.ServiceBus)
            .Select(i => i.ToEnumValue())
            .ToList();
        messagingIntegrations.Insert(0, AppConstant.NoneOption);
        var messagingIntegration = AppConsole.MultiSelectionPrompt("Messaging", messagingIntegrations);
        if (!messagingIntegration.Contains(AppConstant.NoneOption))
            selectedIntegrations.AddRange(messagingIntegration.Select(m => m.FromEnumValue<IntegrationType>()));

        var cacheIntegrations = availableIntegrations
            .Where(i => i == IntegrationType.Redis || i == IntegrationType.InMemory)
            .Select(i => i.ToEnumValue())
            .ToList();
        cacheIntegrations.Insert(0, AppConstant.NoneOption);
        var cacheIntegration = AppConsole.MultiSelectionPrompt("Caching", cacheIntegrations);
        if (!cacheIntegration.Contains(AppConstant.NoneOption))
            selectedIntegrations.AddRange(cacheIntegration.Select(c => c.FromEnumValue<IntegrationType>()));

        var paymentIntegrations = availableIntegrations
            .Where(i => i == IntegrationType.Paypal || i == IntegrationType.Stripe)
            .Select(i => i.ToEnumValue())
            .ToList();
        paymentIntegrations.Insert(0, AppConstant.NoneOption);
        var paymentIntegration = AppConsole.MultiSelectionPrompt("Payment", paymentIntegrations);
        if (!paymentIntegration.Contains(AppConstant.NoneOption))
            selectedIntegrations.AddRange(paymentIntegration.Select(p => p.FromEnumValue<IntegrationType>()));

        var includeUnitTest = AppConsole.Prompt("Include Unit tests?", false);
        var includeIntegrationTest = AppConsole.Prompt("Include Integration tests?", false);
        var includeArchitectureTest = AppConsole.Prompt("Include Architecture tests?", false);

        var solutionName = projectName.Replace(" ", string.Empty);
        return new ProjectConfiguration
        {
            ProjectName = solutionName,
            Architecture = architecture.FromEnumValue<ArchitectureType>(),
            Framework = framework.FromEnumValue<NetFramework>(),
            Authentication = !string.IsNullOrEmpty(authentication)
                ? authentication.FromEnumValue<AuthenticationType>()
                : null,
            UseControllers = useControllers.HasValue && useControllers.Value,
            DbContextName = dbContextName,
            Database = dbProvider.FromEnumValue<DatabaseProvider>(),
            UseNetAspire = useNetAspire,
            Integrations = selectedIntegrations,
            IncludeUnitTests = includeUnitTest,
            IncludeIntegrationTests = includeIntegrationTest,
            IncludeArchitectureTests = includeArchitectureTest,
        };
    }
}
