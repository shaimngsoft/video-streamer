using System;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

[assembly: FunctionsStartup(typeof(RadioArchive.Startup))]

namespace RadioArchive
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

            ISettings settings = new Settings
            {
                AzureWebJobsStorage = Environment.GetEnvironmentVariable("AzureWebJobsStorage", EnvironmentVariableTarget.Process),
                AccountName = Environment.GetEnvironmentVariable("AccountName", EnvironmentVariableTarget.Process),
                ResourceGroup = Environment.GetEnvironmentVariable("ResourceGroup", EnvironmentVariableTarget.Process),
                SubscriptionId = Environment.GetEnvironmentVariable("SubscriptionId", EnvironmentVariableTarget.Process),
                DefaultStreamingEndpointName = Environment.GetEnvironmentVariable("DefaultStreamingEndpointName", EnvironmentVariableTarget.Process),
                StreamingLocatorScheme = Environment.GetEnvironmentVariable("StreamingLocatorScheme", EnvironmentVariableTarget.Process),
                AssetStorageAccountName = Environment.GetEnvironmentVariable("AssetStorageAccountName", EnvironmentVariableTarget.Process),
                StreamingTransformName = Environment.GetEnvironmentVariable("StreamingTransformName", EnvironmentVariableTarget.Process)
            };

            if (bool.TryParse(Environment.GetEnvironmentVariable("AutoProcessStreamingLocator", EnvironmentVariableTarget.Process), out bool autoLocator))
                settings.AutoProcessStreamingLocator = autoLocator;
            if (bool.TryParse(Environment.GetEnvironmentVariable("DeleteJobs", EnvironmentVariableTarget.Process), out bool delJobs))
                settings.DeleteJobs = delJobs;
            if (int.TryParse(Environment.GetEnvironmentVariable("AssetExpiryHours", EnvironmentVariableTarget.Process), out int expiry))
                settings.AssetExpiryHours = expiry;
            else
                settings.AssetExpiryHours = 168;


            //config.GetSection(Settings.MediaSettings).Bind(settings);
            Console.Write($"ISettings Configurations: {settings}");


            builder.Services.AddTransient<IStreamingLocatorGenerator, StreamingLocatorGenerator>();
            builder.Services.AddSingleton<ISettings>(settings);
            builder.Services.AddSingleton<IConfiguration>(config);
        }
    }
}

