namespace BlobMetadata.Configuration
{
    public class Settings : ISettings
    {
        public string AzureWebJobsStorage { get; set; }
        public string[] Excluded { get; set; }
        public bool AutoProcessStreamingLocator { get; set; }
        public MediaServicesSettings MediaServices { get; set; }
    }
}
