namespace UniLx.Infra.Services.ExternalServices.Supabase
{
    public class SupabaseClientOptions
    {
        public const string Section = "SupabaseSettings";

        public string? Url { get; set; } = string.Empty;
        public string? PrivateKey { get; set; } = string.Empty;
    }
}
