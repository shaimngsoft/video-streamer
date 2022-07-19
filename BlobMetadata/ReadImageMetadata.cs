using System.IO;
using System.Threading.Tasks;
using BlobMetadata.Configuration;
using BlobMetadata.Extensions;
using ImageMetadata.Transform;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace BlobMetadata
{
    public class ReadImageMetadata
    {
        private readonly ISettings settings;
        private readonly ILogger<ReadImageMetadata> logger;

        public ReadImageMetadata(ISettings settings, ILogger<ReadImageMetadata> logger)
        {
            this.settings = settings;
            this.logger = logger;
        }

        [FunctionName("ReadImageMetadata")]
        public async Task Run(
            [BlobTrigger("media/{name}", Connection = "AzureWebJobsStorage")] CloudBlockBlob blob, string name,
            [CosmosDB(
                databaseName: "metadatadb",
                collectionName: "media",
                ConnectionStringSetting = "ConnectionStrings:MetadataDb")] IAsyncCollector<dynamic> document)
        {
            logger.LogInformation($"ReadImageMetadata: C# Blob trigger function Processed blob\n Name:{name}");

            JObject metadata = new JObject(new JProperty("warning", "Video files are not processed for exif information."));
            if (ContentType.Image == blob.Properties.ContentType.ResolveType())
            {
                using (Stream stream = await blob.OpenReadAsync())
                {
                    metadata = await Transformer.Transform(stream, settings.Excluded, blob.Properties.ContentType, logger);
                }
                logger.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {blob.Properties.Length} Bytes");
                logger.LogInformation($"Configuration: {settings.Excluded.Length}");
                logger.LogInformation($"Excluded: {settings.Excluded}");
            }

            JObject blobInfo = GetBlobInfo(blob);
            string etag = JToken.Parse(blob.Properties.ETag).ToString();
            string cid = $"{blob.Container.Name}/{name.ExtractCId()}";
            JObject data = new JObject(
                new JProperty("cid", cid),
                new JProperty("id", etag),
                new JProperty("name", name),
                new JProperty("metadata", metadata),
                new JProperty("blob", blobInfo));

            logger.LogInformation($"ReadImageMetadata: {name} Size: {blob.Properties.Length} Bytes, data: {data}");
            await document.AddAsync(data);
        }

        private JObject GetBlobInfo(CloudBlockBlob blob)
        {
            return new JObject(
                    new JProperty("name", blob.Name),
                    new JProperty("container", blob.Container.Name),
                    new JProperty("length", blob.Properties.Length),
                    new JProperty("type", blob.BlobType.ToString()),
                    new JProperty("state", blob.Properties.LeaseState.ToString()),
                    new JProperty("status", blob.Properties.LeaseStatus.ToString()),
                    new JProperty("contenttype", blob.Properties.ContentType),
                    new JProperty("lastmodified", blob.Properties.LastModified.ToString())
                    );
        }
    }
}
