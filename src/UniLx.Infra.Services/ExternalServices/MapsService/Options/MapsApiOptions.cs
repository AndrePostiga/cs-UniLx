namespace UniLx.Infra.Services.ExternalServices.MapsService.Options
{
    public class MapsApiOptions
    {
        public const string Section = "MapsApi";

        public string? Url { get; set; } = string.Empty;
        public string? ApiKey { get; set; } = string.Empty;
        public int TimeoutInSeconds { get; set; } = 30;
    }
}
