using AspireCraft.Core.Enums;
using AspireCraft.Core.Extensions;
using AspireCraft.Core.Models;

namespace AspireCraft.CLI.Prompts;

public class PromptBuilder
{
    private readonly List<PromptDefinition> _prompts = new();

    public PromptBuilder AddProjectSetupPrompts()
    {
        _prompts.Add(new PromptDefinition<string>("projectName", "Project Name", PromptType.Text));

        _prompts.Add(new PromptDefinition<string>("framework", "Target Framework", PromptType.SingleSelect,
            EnumExtensions.GetEnumValues<TargetFramework>(), defaultValue: TargetFramework.DotNet8.GetDisplayName(),
            disabledOptions: new[] { TargetFramework.DotNet10.GetDisplayName() }));

        return this;
    }

    public PromptBuilder AddArchitecturePrompts()
    {
        _prompts.Add(new PromptDefinition<string>("architecture", "Select Architecture", PromptType.SingleSelect,
            EnumExtensions.GetEnumValues<ArchitectureType>(), defaultValue: ArchitectureType.CleanArchitecture.GetDisplayName(),
            disabledOptions: new[] { ArchitectureType.VerticalSliceArchitecture.GetDisplayName() }));

        return this;
    }

    public PromptBuilder AddDatabasePrompts()
    {
        _prompts.Add(new PromptDefinition<string>("database", "Database Provider", PromptType.SingleSelect,
           EnumExtensions.GetEnumValues<DatabaseProvider>(), defaultValue: DatabaseProvider.PostgreSQL.GetDisplayName()));

        return this;
    }

    public PromptBuilder AddIntegrationPrompts()
    {
        _prompts.Add(new PromptDefinition<string>("email", "Email Provider", PromptType.SingleSelect,
           EnumExtensions.GetEnumValues<EmailProvider>(), defaultValue: EmailProvider.None.GetDisplayName()));

        _prompts.Add(new PromptDefinition<string>("sms", "SMS Provider", PromptType.SingleSelect,
           EnumExtensions.GetEnumValues<SmsProvider>(), defaultValue: SmsProvider.None.GetDisplayName()));

        _prompts.Add(new PromptDefinition<string>("payment", "Payment Provider", PromptType.SingleSelect,
           EnumExtensions.GetEnumValues<PaymentProvider>(), defaultValue: PaymentProvider.None.GetDisplayName()));

        // aspire service
        _prompts.Add(new PromptDefinition<string>("cache", "Caching Strategy", PromptType.SingleSelect,
           EnumExtensions.GetEnumValues<CachingStrategy>(), defaultValue: CachingStrategy.None.GetDisplayName()));

        _prompts.Add(new PromptDefinition<string>("messaging", "Message Broker", PromptType.SingleSelect,
           EnumExtensions.GetEnumValues<MessageBroker>(), defaultValue: MessageBroker.None.GetDisplayName()));

        _prompts.Add(new PromptDefinition<string>("storage", "Storage Provider", PromptType.SingleSelect,
           EnumExtensions.GetEnumValues<StorageProvider>(), defaultValue: StorageProvider.None.GetDisplayName()));

        return this;
    }

    public PromptBuilder AddSecurityPrompts()
    {
        _prompts.Add(new PromptDefinition<string>("authentication", "Authentication", PromptType.SingleSelect,
           EnumExtensions.GetEnumValues<AuthenticationType>(), defaultValue: AuthenticationType.JwtBearer.GetDisplayName()));

        return this;
    }

    public PromptBuilder AddTestPrompts()
    {
        _prompts.Add(new PromptDefinition<bool>("integrationTest", "Include Aspire Integration Tests?", PromptType.Boolean,
            defaultValue: false));

        _prompts.Add(new PromptDefinition<bool>("architectureTest", "Include Architecture Tests?", PromptType.Boolean,
            defaultValue: false));

        return this;
    }

    public List<PromptDefinition> Build()
    {
        return _prompts;
    }
}
