using AspireCraft.Core.Auth;
using AspireCraft.Core.Cache;
using AspireCraft.Core.Common.Abstractions;
using AspireCraft.Core.Common.Enums;
using AspireCraft.Core.Common.Models;
using AspireCraft.Core.Database;
using AspireCraft.Core.Mailer.Providers;
using AspireCraft.Core.Payments;
using AspireCraft.Core.Renderers;
using AspireCraft.Core.Sms;
using AspireCraft.Core.Storage;

namespace AspireCraft.Core.Base;

public sealed class CleanArchitecture : ITemplateArchitecture
{
    public string Name => "CleanArchitecture";
    private readonly List<IPackageInstaller> _packages = [];

    public CleanArchitecture()
    {
        _packages = new List<IPackageInstaller>
        {
            // database
            new SqlServerInstaller(),
            new PostgreSqlInstaller(),
            new MySqlInstaller(),
            new SqliteInstaller(),
            new MongoDbInstaller(),

            // cache
            new RedisCacheInstaller(),
            new InMemoryCacheInstaller(),

            // auth
            new JwtInstaller(),
            new Auth0Installer(),
            new DuendeIdentityInstaller(),

            // email
            new SendGridInstaller(),
            new MailgunInstaller(),

            // sms
            new TwilioInstaller(),
            new WavecellInstaller(),

            // payments
            new StripeInstaller(),
            new PaypalInstaller(),

            // storage
            new AzureBlobInstaller(),
            new AwsS3BucketInstaller(),
        };
    }

    public void Generate(ProjectConfiguration project, TemplateContext context)
    {
        Directory.CreateDirectory(Path.Combine(context.RootPath, "src", "Domain"));
        Directory.CreateDirectory(Path.Combine(context.RootPath, "src", project.Architecture == ArchitectureType.NLayer ? "Business" : "Application"));
        Directory.CreateDirectory(Path.Combine(context.RootPath, "src", project.Architecture == ArchitectureType.NLayer ? "Data" : "Infrastructure"));
        Directory.CreateDirectory(Path.Combine(context.RootPath, "src", project.Architecture == ArchitectureType.Serverless ? "Function" : "Api"));

        foreach (var package in _packages.Where(p => p.CanInstall(project)))
        {
            package.Install(project, context);
        }
    }
}
