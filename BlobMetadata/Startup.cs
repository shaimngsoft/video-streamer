using BlobMetadata.Configuration;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

[assembly: FunctionsStartup(typeof(BlobMetadata.Startup))]

namespace BlobMetadata
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            ExecutionContextOptions options = builder.Services.BuildServiceProvider()
                .GetService<IOptions<ExecutionContextOptions>>().Value;

            IConfigurationRoot config = new ConfigurationBuilder()
                .SetBasePath(options.AppDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            Settings settings = new Settings
            {
                Excluded = config.GetSection("Metadata:Excluded").Get<string[]>(),
                AutoProcessStreamingLocator = config.GetSection("Metadata:AutoProcessStreamingLocator").Get<bool>(),
                AzureWebJobsStorage = config["Values:AzureWebJobsStorage"]
            };

            MediaServicesSettings media = new MediaServicesSettings();
            config.GetSection("MediaServices").Bind(media);
            settings.MediaServices = media;
            builder.Services.AddTransient<IStreamingLocatorGenerator, StreamingLocatorGenerator>();
            builder.Services.AddSingleton<ISettings>(settings);
            builder.Services.AddSingleton<IConfiguration>(config);
        }
    }
}
