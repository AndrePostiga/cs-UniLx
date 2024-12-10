namespace UniLx.Infra.Data.Storage.Buckets
{
    public interface IBucketSettings
    {
        public string? BucketName { get; set; }
        public string? Folder { get; set; }
    }
}
