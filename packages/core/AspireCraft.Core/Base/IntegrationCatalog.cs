using AspireCraft.Core.Common.Enums;
using AspireCraft.Core.Common.Extensions;

namespace AspireCraft.Core.Base;

public static class IntegrationCatalog
{
    private static Dictionary<IntegrationType, bool> _integrations = new()
    {
         // Database
        { IntegrationType.SqlServer, false },
        { IntegrationType.PostgreSQL, false },
        { IntegrationType.MySQL, false },
        { IntegrationType.SQLite, false },
        { IntegrationType.MongoDB, true },

        // Auth
        { IntegrationType.Jwt, false },
        { IntegrationType.Auth0, false },
        { IntegrationType.DuendeIdentity, false },
        
        // Email
        { IntegrationType.SendGrid, false },
        { IntegrationType.Mailgun, false },

        // SMS
        { IntegrationType.Wavecell, false },
        { IntegrationType.Twilio, false },

        // Storage
        { IntegrationType.AzureBlob, false },
        { IntegrationType.AwsS3Bucket, false },
        
        // Messaging
        { IntegrationType.RabbitMQ, true },
        { IntegrationType.Kafka, true },
        { IntegrationType.ServiceBus, true },

        // Cache
        { IntegrationType.Redis, true },
        { IntegrationType.InMemory, false },

        // Payment
        { IntegrationType.Paypal, true },
        { IntegrationType.Stripe, true },
    };

    public static IReadOnlyList<IntegrationType> GetAvailableIntegrations(bool aspireEnabled)
    {
        return _integrations
            .Where(kvp => !kvp.Value || aspireEnabled)
            .Select(kvp => kvp.Key)
            .ToList()
            .AsReadOnly();
    }

    public static bool RequireNetAspire(IntegrationType integration)
    {
        if (!_integrations.ContainsKey(integration))
            throw new ArgumentException($"Integration '{integration.ToEnumValue}' does not exist in catalog.");

        return _integrations[integration];
    }
}
