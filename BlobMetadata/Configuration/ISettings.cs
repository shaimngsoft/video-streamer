namespace BlobMetadata.Configuration
{
    public interface ISettings
    {
        string AzureWebJobsStorage { get; set; }
        string[] Excluded { get; set; }
        bool AutoProcessStreamingLocator { get; set; }
        MediaServicesSettings MediaServices { get; set; }
    }
}
