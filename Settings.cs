using System;
namespace RadioArchive
{
    public class Settings : ISettings
    {
        public const string MediaSettings = "Values";

        public string   AzureWebJobsStorage             { get; set; }
        public bool     AutoProcessStreamingLocator     { get; set; }
        public string   AccountName                     { get; set; }
        public string   ResourceGroup                   { get; set; }
        public string   SubscriptionId                  { get; set; }
        public double   AssetExpiryHours                { get; set; }
        public string   DefaultStreamingEndpointName    { get; set; }
        public string   StreamingLocatorScheme          { get; set; }
        public string   AssetStorageAccountName         { get; set; }
        public bool     DeleteJobs                      { get; set; }
        public string   StreamingTransformName          { get; set; }

        public override string ToString()
        {
            return $"AzureWebJobsStorage: {AzureWebJobsStorage}, AutoProcessStreamingLocator: {AutoProcessStreamingLocator}, AccountName: {AccountName}, ResourceGroup: {ResourceGroup},  SubscriptionId: {SubscriptionId}, AssetExpiryHours: {AssetExpiryHours}, AssetStorageAccountName: {AssetStorageAccountName}, DeleteJobs: {DeleteJobs}, StreamingTransformName: {StreamingTransformName}";
        }
    }
}

