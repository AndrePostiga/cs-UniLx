using System.Text.Json.Serialization;

namespace UniLx.Infra.Services.ExternalServices.Cognito
{
    public class UserInfoResponse
    {
        [JsonPropertyName("sub")]
        public string? Subscription {get;set;}

        [JsonPropertyName("email_verified")]
        public string? EmailVerified {get;set;}

        [JsonPropertyName("email")]
        public string? Email {get;set;}

        [JsonPropertyName("username")]
        public string? UserName { get; set; }
    }
}
