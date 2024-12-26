namespace UniLx.Infra.Data.Storage.Buckets
{
    public class AdvertisementBucketOptions : IBucketSettings
    {
        public const string Section = "AdvertisementBucket";

        public string? BucketName { get; set; } = string.Empty;
        public string? Folder { get; set; } = string.Empty;
        public bool IsPrivate { get; set; }
    }
}
