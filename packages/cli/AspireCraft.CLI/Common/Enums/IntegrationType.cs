using AspireCraft.CLI.Common.Extensions;

namespace AspireCraft.CLI.Common.Enums;

public enum IntegrationType
{
    /// <summary>
    /// Database type
    /// </summary>
    [DisplayName("SQL Server")]
    SqlServer,

    [DisplayName("PostgreSQL")]
    PostgreSQL,

    [DisplayName("MySQL")]
    MySQL,

    [DisplayName("SQLite")]
    SQLite,

    [DisplayName("MongoDB")]
    MongoDB,


    /// <summary>
    /// Authentication
    /// </summary>
    [DisplayName("JWT")]
    Jwt,

    [DisplayName("Auth0")]
    Auth0,

    [DisplayName("Duende Identity")]
    DuendeIdentity,


    /// <summary>
    /// Email providers
    /// </summary>
    [DisplayName("SendGrid")]
    SendGrid,

    [DisplayName("Mailgun")]
    Mailgun,


    /// <summary>
    /// SMS providers
    /// </summary>
    [DisplayName("Wavecell")]
    Wavecell,

    [DisplayName("Twilio")]
    Twilio,


    /// <summary>
    /// Storage type
    /// </summary>
    [DisplayName("Azure Blob")]
    AzureBlob,

    [DisplayName("S3 Bucket")]
    AwsS3Bucket,


    /// <summary>
    /// Messaging
    /// </summary>
    [DisplayName("RabbitMQ")]
    RabbitMQ,

    [DisplayName("Kafka")]
    Kafka,

    [DisplayName("Service Bus")]
    ServiceBus,


    /// <summary>
    /// Caching options
    /// </summary>
    [DisplayName("Redis")]
    Redis,

    [DisplayName("In-Memory")]
    InMemory,


    /// <summary>
    /// Payment providers
    /// </summary>
    [DisplayName("Paypal")]
    Paypal,

    [DisplayName("Stripe")]
    Stripe,
}
