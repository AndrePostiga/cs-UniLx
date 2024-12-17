using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using UniLx.ApiService.Controllers.Accounts;
using UniLx.Application.Usecases.Accounts.Queries.GetAccountByCognitoSub.Internal;
using UniLx.Domain.Entities.AccountAgg;
using UniLx.Infra.Services.ExternalServices.Cognito;
using UniLx.Shared.Abstractions;

namespace UniLx.ApiService.Authorization
{
    public class JwtAuthHandlerDecorator : JwtBearerHandler
    {
        private readonly IMediator _mediator;
        private readonly IRequestContext _requestContext;
        private readonly ICognitoService _cognitoService;

#pragma warning disable CS0618 // O tipo ou membro é obsoleto
        public JwtAuthHandlerDecorator(
            IOptionsMonitor<JwtBearerOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IMediator mediator,
            IRequestContext requestContext,
            ICognitoService cognitoService) : base(options, logger, encoder, clock)
#pragma warning restore CS0618 // O tipo ou membro é obsoleto
        {
            _mediator = mediator;
            _requestContext = requestContext;
            _cognitoService = cognitoService;
        }

        protected async override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var result = await base.HandleAuthenticateAsync();
            if (!result.Succeeded) return result;

            var accessToken = result.Ticket?.Properties?.GetTokenValue("access_token");
            if (string.IsNullOrEmpty(accessToken))
                return FailWithMessage("Access token not found");

            var userInfo = await _cognitoService.GetUserInfo($"Bearer {accessToken}");
            var userSub = ExtractClaim(result.Principal, "username");
            var userGroup = ExtractClaim(result.Principal, "cognito:groups");

            if (string.IsNullOrEmpty(userSub) || userInfo is null || string.IsNullOrEmpty(userGroup))
                return FailWithMessage("User information is missing in the token.");

            var endpoint = _requestContext.HttpContext?.GetEndpoint();
            var isCreateAccountEndpoint = endpoint?.Metadata.GetMetadata<EndpointNameMetadata>()?.EndpointName == nameof(AdminControllerHandlers.CreateAccount);

            var account = await _mediator.Send(new GetAccountByCognitoSubQueryInternal(userSub));

            if (account is null)
            {
                if (!isCreateAccountEndpoint)
                    return FailWithMessage("User registration is required.");

                AppendClaims(result.Principal, null, userGroup, userSub, userInfo.Email);
                AddAccountDataToContext(null, userGroup, userSub, userInfo.Email);
                return AuthenticateResult.Success(result.Ticket!);
            }

            if (isCreateAccountEndpoint)
                return FailWithMessage("User already been registered.");

            AppendClaims(result.Principal, account, userGroup, userSub, userInfo.Email);
            AddAccountDataToContext(account, userGroup, userSub, userInfo.Email);

            return result;
        }

        private AuthenticateResult FailWithMessage(string message)
        {
            _requestContext.HttpContext!.Items["failureMessage"] = message;
            return AuthenticateResult.Fail(message);
        }

        private void AddAccountDataToContext(Account? account, string? group, string? cognitoSub, string? email)
        {
            _requestContext.AccountId = account?.Id;
            _requestContext.Email = account?.Email?.Value ?? email;
            _requestContext.CognitoIdentifier = account?.CognitoSubscriptionId ?? cognitoSub;
            _requestContext.CognitoGroup = group;
        }

        private void AppendClaims(ClaimsPrincipal principal, Account? account, string? group, string? cognitoSub, string? email)
        {
            var identity = principal.Identity as ClaimsIdentity ?? throw new InvalidOperationException("The ClaimsPrincipal does not have a valid ClaimsIdentity.");

            if (!string.IsNullOrEmpty(account?.Id))
                identity.AddClaim(new Claim(nameof(account.Id), account.Id));

            var cognitoSubValue = account?.CognitoSubscriptionId ?? cognitoSub;
            if (!string.IsNullOrEmpty(cognitoSubValue))
                identity.AddClaim(new Claim(nameof(account.CognitoSubscriptionId), cognitoSubValue));

            if (!string.IsNullOrEmpty(group))
                identity.AddClaim(new Claim("CognitoGroup", group));

            if (!string.IsNullOrEmpty(email))
                identity.AddClaim(new Claim(ClaimTypes.Email, email));
        }

        private static string? ExtractClaim(ClaimsPrincipal principal, string claimType)
        {
            return principal.Claims.FirstOrDefault(c => c.Type == claimType)?.Value;
        }

        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            var failureMessage = _requestContext.HttpContext!.Items.TryGetValue("failureMessage", out var message) && message is string msg
                ? msg
                : "Authentication failed.";

            var problemDetails = new ProblemDetails
            {
                Title = "Authentication Failed",
                Status = StatusCodes.Status401Unauthorized,
                Detail = failureMessage,
                Instance = _requestContext.HttpContext!.Request.Path
            };

            _requestContext.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            _requestContext.HttpContext.Response.ContentType = "application/json";

            return _requestContext.HttpContext.Response.WriteAsJsonAsync(problemDetails);
        }

        protected override Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            var problemDetails = new ProblemDetails
            {
                Title = "Access Denied",
                Status = StatusCodes.Status403Forbidden,
                Detail = "You do not have permission to access this resource.",
                Instance = _requestContext.HttpContext!.Request.Path
            };

            _requestContext.HttpContext!.Response.StatusCode = StatusCodes.Status403Forbidden;
            _requestContext.HttpContext!.Response.ContentType = "application/json";

            return _requestContext.HttpContext.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}
