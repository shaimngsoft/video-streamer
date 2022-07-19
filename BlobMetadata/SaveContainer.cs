using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Net;

namespace BlobMetadata
{
    public static class SaveContainer
    {
        [FunctionName("SaveContainer")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", "put", Route = null)] HttpRequest req,
            [CosmosDB(
                databaseName: "metadatadb",
                collectionName: "containers",
                ConnectionStringSetting = "ConnectionStrings:MetadataDb", CreateIfNotExists = true)]IAsyncCollector<dynamic> document,
            ILogger logger)
        {
            logger.LogInformation("C# HTTP trigger function processed a request.");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            logger.LogInformation(requestBody);
            dynamic container = JsonConvert.DeserializeObject<object>(requestBody);

            await document.AddAsync(container);

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(container.ToString(), Encoding.UTF8, "application/json")
            };
        }
    }
}
