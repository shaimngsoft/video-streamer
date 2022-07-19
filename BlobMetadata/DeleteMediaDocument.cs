using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System.Text;

namespace BlobMetadata
{
    public static class DeleteMediaDocument
    {
        [FunctionName("DeleteMediaDocument")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = null)] HttpRequest req,
            [CosmosDB(
                databaseName: "metadatadb", 
                collectionName: "media", Id = "{Query.id}", PartitionKey = "{Query.partitionKey}", 
                ConnectionStringSetting = "ConnectionStrings:MetadataDb")] Document document,
            [CosmosDB(
                databaseName: "metadatadb", 
                collectionName: "media",
                ConnectionStringSetting = "ConnectionStrings:MetadataDb")] DocumentClient client,
            ILogger logger)
        {
            logger.LogInformation("C# HTTP trigger function processed a request.");

            string partitionKey = req.Query["partitionKey"];

            if (document == null || string.IsNullOrEmpty(partitionKey))
                return new HttpResponseMessage(HttpStatusCode.BadRequest);

            await client.DeleteDocumentAsync(document.SelfLink, new RequestOptions() { PartitionKey = new PartitionKey(partitionKey) });

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(document.ToString(), Encoding.UTF8, "application/json")
            };
        }
    }
}
