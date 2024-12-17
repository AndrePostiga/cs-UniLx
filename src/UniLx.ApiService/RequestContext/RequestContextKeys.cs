using System.Diagnostics.CodeAnalysis;

namespace UniLx.ApiService.RequestContext
{
    [ExcludeFromCodeCoverage]
    public static class RequestContextKeys
    {
        public const string RequestKey = "RequestKey";
        public const string CognitoIdentifier = "X-Identifier";
        public const string CognitoGroup = "X-Group";
        public const string Account = "X-Account";
        public const string AccountId = "AccountId";
        public const string Email = "Email";
        public const string XForwardedFor = "X-Forwarded-For";
        public const string IdToken = "X-IdToken";
    }
}
