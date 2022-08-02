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

            ISettings settings = new Settings();
            config.GetSection(Settings.MediaSettings).Bind(settings);
            builder.Services.AddTransient<IStreamingLocatorGenerator, StreamingLocatorGenerator>();
            builder.Services.AddSingleton<ISettings>(settings);
            builder.Services.AddSingleton<IConfiguration>(config);
        }
    }
}

