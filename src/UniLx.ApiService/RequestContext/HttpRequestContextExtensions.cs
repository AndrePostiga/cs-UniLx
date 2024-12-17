using Microsoft.Extensions.Primitives;
using System.Diagnostics.CodeAnalysis;
using UniLx.Domain.Entities.AccountAgg;

namespace UniLx.ApiService.RequestContext
{
    [ExcludeFromCodeCoverage]
    public static class HttpRequestContextExtensions
    {
        public static string? GetIdentifier(this HttpContext context)
        {
            if (context.Request?.Headers is null)
                return null;

            if (context.Request.Headers.TryGetValue(RequestContextKeys.CognitoIdentifier, out StringValues value))
                return value.First();

            return null;
        }

        public static string? GetCognitoGroup(this HttpContext context)
        {
            if (context.Request?.Headers is null)
                return null;

            if (context.Request.Headers.TryGetValue(RequestContextKeys.CognitoGroup, out StringValues value))
                return value.First();

            return null;
        }

        public static Account? GetAccount(this HttpContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            
            if (context.Items == null || !context.Items.ContainsKey("X-Account"))
                return null;
            
            if (context.Items["X-Account"] is Account account)
                return account;

            return null;
        }

        public static string? GetIdToken(this HttpContext context)
        {
            if (context.Request?.Headers is null)
                return null;

            if (context.Request.Headers.TryGetValue(RequestContextKeys.IdToken, out StringValues value))
                return value.First();

            return null;
        }

    }
}
