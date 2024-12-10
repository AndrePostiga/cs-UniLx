namespace UniLx.Infra.Data.Storage.Buckets
{
    public class AccountAvatarBucketOptions : IBucketSettings
    {
        public const string Section = "AccountAvatarBucket";

        public string? BucketName { get; set; } = string.Empty;
        public string? Folder { get; set; } = string.Empty;
    }
}
