namespace UniLx.Infra.Data.Database.Options
{
    public class DatabaseOptions
    {
        public const string Section = "Database";

        public string? ConnectionString { get; set; } = string.Empty;
    }
}
