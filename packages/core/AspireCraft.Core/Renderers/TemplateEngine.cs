using AspireCraft.Core.Auth;
using AspireCraft.Core.Base;
using AspireCraft.Core.Cache;
using AspireCraft.Core.Common.Abstractions;
using AspireCraft.Core.Common.Models;
using AspireCraft.Core.Database;
using AspireCraft.Core.Mailer.Providers;
using AspireCraft.Core.Messaging;
using AspireCraft.Core.Payments;
using AspireCraft.Core.Sms;
using AspireCraft.Core.Storage;

namespace AspireCraft.Core.Renderers;

public sealed class TemplateEngine
{
    private readonly IEnumerable<IProjectTemplate> _templates;
    private readonly IEnumerable<IProjectRenderer> _renderers;

    private TemplateEngine(IEnumerable<IProjectRenderer> renderers, IEnumerable<IPackageInstaller> installers)
    {
        _renderers = renderers;
        _templates = _renderers.Select(t => new GenericTemplate(t, installers)).ToList();
    }

    public static TemplateEngine CreateDefault()
    {
        var renderers = new List<IProjectRenderer>()
        {
            new CleanArchitectureRenderer(),
            new ServerlessRenderer(),
            new NLayerRenderer(),
        };

        var installer = new List<IPackageInstaller>
        {
            new SqlServerInstaller(),   // database
            new PostgreSqlInstaller(),
            new MySqlInstaller(),
            new SqliteInstaller(),
            new MongoDbInstaller(),
            new RedisCacheInstaller(),  // cache
            new InMemoryCacheInstaller(),
            new JwtInstaller(),         // auth
            new Auth0Installer(),
            new DuendeIdentityInstaller(),
            new SendGridInstaller(),    // email
            new MailgunInstaller(),
            new TwilioInstaller(),      // sms
            new WavecellInstaller(),
            new StripeInstaller(),      // payments
            new PaypalInstaller(),
            new AzureBlobInstaller(),   // storage
            new AwsS3BucketInstaller(),
            new RabbitMqInstaller(),    // messaging
            new KafkaInstaller(),
            new ServiceBusInstaller(),
        };

        return new TemplateEngine(renderers, installer);
    }

    public void Generate(ProjectConfiguration configuration)
    {
        var renderer = _renderers.FirstOrDefault(t => t.Architecture == configuration.Architecture);
        if (renderer == null)
            throw new InvalidOperationException($"Renderer for {configuration.Architecture} not found");

        var template = _templates.FirstOrDefault(t => t.Architecture == configuration.Architecture);
        if (template == null)
            throw new FileNotFoundException($"Template '{configuration.Architecture}' not found");

        var context = new TemplateContext(configuration, renderer);
        template.Generate(configuration, context);
    }
}
