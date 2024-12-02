using Microsoft.AspNetCore.Http;
using System.Net;

namespace UniLx.Shared.Abstractions
{
    public sealed record Error(HttpStatusCode StatusCode, string Code, string? Description = null)
    {
        public static readonly Error None = new(HttpStatusCode.OK, string.Empty);

        public static implicit operator ValidationProblemDetails(Error error) => new(error);

        public IResult ToBadRequest()
        {
            var problem = new ValidationProblemDetails(this);
            return Results.Json(
                problem,
                statusCode: problem.Status,
                contentType: "application/problem+json"
            );
        }        
    }
}
