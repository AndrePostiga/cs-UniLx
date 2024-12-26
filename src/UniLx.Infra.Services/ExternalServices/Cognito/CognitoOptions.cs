namespace UniLx.Infra.Services.ExternalServices.Cognito
{
    public class CognitoOptions
    {
        public const string Section = "Aws:CognitoOptions";

        public string? Authority { get; set; } = string.Empty;
        public string? Domain { get; set; } = string.Empty;
        public string? MetadataAddress { get; set; } = string.Empty;
        public string? WellKnown { get; set; } = string.Empty;
        public string? ClientId { get; set; } = string.Empty;
        public string? RoleClaimType { get; set; } = string.Empty;
    }
}
