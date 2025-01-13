using System.Diagnostics.CodeAnalysis;
using UniLx.Shared.Abstractions;

namespace UniLx.ApiService.RequestContext
{
    [ExcludeFromCodeCoverage]
    public class RequestContext(IHttpContextAccessor httpContextAccessor) : IRequestContext
    {
        private readonly IHttpContextAccessor _contextAccessor = httpContextAccessor;
        public IHttpContextAccessor? ContextAccessor => _contextAccessor;
        public HttpContext? HttpContext => _contextAccessor?.HttpContext;

        public string? RequestKey
        {
            get => _contextAccessor.HttpContext?.Items[RequestContextKeys.RequestKey]?.ToString();
            set => _contextAccessor.HttpContext!.Items.Add(RequestContextKeys.RequestKey, value);
        }

        public string? ClientIp
        {
            get => _contextAccessor.HttpContext?.Request.Headers.ContainsKey(RequestContextKeys.XForwardedFor) == true
                ? _contextAccessor.HttpContext?.Request.Headers[RequestContextKeys.XForwardedFor].First()?.Split(',').FirstOrDefault()
                : _contextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
        }

        public string? CognitoIdentifier
        {
            get => _contextAccessor.HttpContext?.Items[RequestContextKeys.CognitoIdentifier]?.ToString() ?? _contextAccessor.HttpContext?.GetIdentifier();
            set => _contextAccessor.HttpContext!.Items.Add(RequestContextKeys.CognitoIdentifier, value);
        }

        public string? CognitoGroup
        {
            get => _contextAccessor.HttpContext?.Items[RequestContextKeys.CognitoGroup]?.ToString() ?? _contextAccessor.HttpContext?.GetCognitoGroup();
            set => _contextAccessor.HttpContext!.Items.Add(RequestContextKeys.CognitoGroup, value);
        }

        public string? AccountId
        {
            get => _contextAccessor.HttpContext!.User.FindFirst(RequestContextKeys.AccountId)?.Value ?? _contextAccessor.HttpContext?.Items[RequestContextKeys.AccountId]?.ToString();
            set => _contextAccessor.HttpContext!.Items.Add(RequestContextKeys.AccountId, value);
        }

        public string? Email
        {
            get => _contextAccessor.HttpContext!.User.FindFirst(RequestContextKeys.Email)?.Value ?? _contextAccessor.HttpContext?.Items[RequestContextKeys.Email]?.ToString();
            set => _contextAccessor.HttpContext!.Items.Add(RequestContextKeys.Email, value);
        }
    }
}
