using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;

namespace BlobMetadata.Extensions
{
    public static class ExecutionContextExtensions
    {
        public static IConfiguration GetConfuguration(this ExecutionContext context)
        {
            IConfigurationRoot config = new ConfigurationBuilder()
                            .SetBasePath(context.FunctionAppDirectory)
                            .AddJsonFile("local.settings.json", true, true)
                            .AddEnvironmentVariables().Build();
            return config;
        }
    }
}
