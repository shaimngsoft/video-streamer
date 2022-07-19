using System;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using BlobMetadata.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace BlobMetadata
{
    public class GenerateSasToken
    {
        private readonly ISettings settings;
        public GenerateSasToken(ISettings settings)
        {
            this.settings = settings;
        }

        [FunctionName("GenerateSasToken")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get" , Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            return await Task<IActionResult>.Factory.StartNew(() =>
            {
                BlobServiceClient client = new BlobServiceClient(settings.AzureWebJobsStorage);
                if (client.CanGenerateAccountSasUri)
                {
                    Uri sasUri = client.GenerateAccountSasUri(
                        AccountSasPermissions.All,
                        DateTimeOffset.UtcNow.AddHours(2),
                        AccountSasResourceTypes.All);
                    string[] sasParts = sasUri.ToString().Split('?');

                    JObject token = new JObject(
                        new JProperty("storageUri", sasParts[0]),
                        new JProperty("storageAccessToken", sasParts[1]));


                    return new OkObjectResult(token);
                }
                return new BadRequestResult();
            });

        }
    }
}

