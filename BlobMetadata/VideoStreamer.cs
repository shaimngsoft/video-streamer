using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BlobMetadata.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Management.Media.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BlobMetadata
{
    public class VideoStreamer
    {
        private readonly ISettings settings;
        private readonly ILogger<VideoStreamer> logger;
        private readonly IStreamingLocatorGenerator generator;

        public VideoStreamer(ISettings settings, IStreamingLocatorGenerator generator, ILogger<VideoStreamer> logger)
        {
            this.settings = settings;
            this.generator = generator;
            this.logger = logger;
        }

        [FunctionName("VideoStreamer")]
        public async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest request,
            [Blob("media/{Query.name}", FileAccess.Read, Connection = "AzureWebJobsStorage")] Stream blob)
        {
            logger.LogInformation("C# HTTP trigger function processed a request.");

            string name = request.Query["name"];
            logger.LogInformation($"Blob name {name}, blob length {blob.Length}");

            IDictionary<string, StreamingPath> urls = await generator.Generate(name, blob);
            logger.LogInformation($"VideoStreamer urls: {urls}");

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(urls), Encoding.UTF8, "application/json")
            };

        }

    }
}

