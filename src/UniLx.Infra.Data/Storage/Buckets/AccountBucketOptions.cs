namespace UniLx.Infra.Data.Storage.Buckets
{
    public class AccountBucketOptions : IBucketSettings
    {
        public const string Section = "AccountBucket";

        public string? BucketName { get; set; } = string.Empty;
        public string? Folder { get; set; } = string.Empty;
        public bool IsPrivate { get; set; }
    }
}
