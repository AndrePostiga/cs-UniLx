using Microsoft.AspNetCore.Http;

namespace UniLx.Shared.Abstractions
{
    public interface IRequestContext
    {
        IHttpContextAccessor? ContextAccessor { get; }
        string? AccountId { get; set; }
        string? RequestKey { get; set; }
        string? CognitoIdentifier { get; set; }
        string? CognitoGroup { get; set; }
        string? ClientIp { get; }
        public HttpContext? HttpContext { get; }
        string? Email { get; set; }
    }
}
