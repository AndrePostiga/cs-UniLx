namespace UniLx.Infra.Data.Storage
{
    public class S3Options
    {
        public const string Section = "Aws:S3Options";

        public string? AccessKeyId { get; set; } = string.Empty;
        public string? SecretAccessKey { get; set; } = string.Empty;
        public string? Region { get; set; } = string.Empty;
    }
}