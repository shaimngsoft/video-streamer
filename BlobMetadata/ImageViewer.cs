using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using BlobMetadata.Configuration;
using Microsoft.Azure.Storage.Blob;

namespace BlobMetadata
{
    public class ImageViewer
    {
        private readonly ISettings settings;

        public ImageViewer(ISettings settings)
        {
            this.settings = settings;
        }

        [FunctionName("ImageViewer")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            [Blob("media/{Query.name}", FileAccess.Read, Connection = "AzureWebJobsStorage")] CloudBlockBlob blob,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            Stream blobStream = await blob.OpenReadAsync().ConfigureAwait(false);
            return new FileStreamResult(blobStream, blob.Properties.ContentType);
        }
    }
}
