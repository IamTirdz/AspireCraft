using AspireCraft.Core.Enums;
using AspireCraft.Core.Extensions;
using AspireCraft.Core.Models;

namespace AspireCraft.CLI.Prompts;

public class PromptBuilder
{
    private readonly List<PromptDefinition> _prompts = new();

    public PromptBuilder AddProjectSetupPrompts()
    {
        _prompts.Add(new PromptDefinition("projectName", "Project Name", PromptType.Text, defaultValue: "MyApp"));

        _prompts.Add(new PromptDefinition("framework", "Target Framework", PromptType.SingleSelect,
            EnumExtensions.GetEnumValues<TargetFramework>(), defaultValue: TargetFramework.DotNet8.GetDisplayName()));

        return this;
    }

    public PromptBuilder AddArchitecturePrompts()
    {
        _prompts.Add(new PromptDefinition("architecture", "Select Architecture", PromptType.SingleSelect,
            EnumExtensions.GetEnumValues<ArchitectureType>(), defaultValue: ArchitectureType.CleanArchitecture.GetDisplayName()));

        return this;
    }

    public PromptBuilder AddDatabasePrompts()
    {
        _prompts.Add(new PromptDefinition("database", "Database Provider", PromptType.SingleSelect,
           EnumExtensions.GetEnumValues<DatabaseProvider>(), defaultValue: DatabaseProvider.PostgreSQL.GetDisplayName()));

        return this;
    }

    public PromptBuilder AddIntegrationPrompts()
    {
        _prompts.Add(new PromptDefinition("email", "Email Provider", PromptType.SingleSelect,
           EnumExtensions.GetEnumValues<EmailProvider>(), defaultValue: EmailProvider.None.GetDisplayName()));

        _prompts.Add(new PromptDefinition("sms", "SMS Provider", PromptType.SingleSelect,
           EnumExtensions.GetEnumValues<SmsProvider>(), defaultValue: SmsProvider.None.GetDisplayName()));

        _prompts.Add(new PromptDefinition("payment", "Payment Provider", PromptType.SingleSelect,
           EnumExtensions.GetEnumValues<PaymentProvider>(), defaultValue: PaymentProvider.None.GetDisplayName()));

        // aspire service
        _prompts.Add(new PromptDefinition("cache", "Caching Strategy", PromptType.SingleSelect,
           EnumExtensions.GetEnumValues<CachingStrategy>(), defaultValue: CachingStrategy.None.GetDisplayName()));

        _prompts.Add(new PromptDefinition("messaging", "Message Broker", PromptType.SingleSelect,
           EnumExtensions.GetEnumValues<MessageBroker>(), defaultValue: MessageBroker.None.GetDisplayName()));

        _prompts.Add(new PromptDefinition("storage", "Storage Provider", PromptType.SingleSelect,
           EnumExtensions.GetEnumValues<StorageProvider>(), defaultValue: StorageProvider.None.GetDisplayName()));

        return this;
    }

    public PromptBuilder AddSecurityPrompts()
    {
        _prompts.Add(new PromptDefinition("authentication", "Authentication", PromptType.SingleSelect,
           EnumExtensions.GetEnumValues<AuthenticationType>(), defaultValue: AuthenticationType.JwtBearer.GetDisplayName()));

        return this;
    }

    public PromptBuilder AddTestPrompts()
    {
        _prompts.Add(new PromptDefinition("testProject", "Test Project", PromptType.MultiSelect,
            EnumExtensions.GetEnumValues<TestProject>(), defaultValue: TestProject.UnitTests.GetDisplayName()));

        _prompts.Add(new PromptDefinition("mockLib", "Mocking Library", PromptType.SingleSelect,
            EnumExtensions.GetEnumValues<MockLibrary>(), defaultValue: MockLibrary.Moq.GetDisplayName()));

        return this;
    }

    public List<PromptDefinition> Build()
    {
        return _prompts;
    }
}
