using Refit;

namespace UniLx.Infra.Services.ExternalServices.Cognito
{
    public interface ICognitoService
    {
        [Get("/oauth2/userinfo")]
        Task<UserInfoResponse?> GetUserInfo([Header("Authorization")] string accessToken, CancellationToken cancellationToken = default);
    }
}
