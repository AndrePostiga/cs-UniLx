using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UniLx.Shared.Abstractions
{
    public class ValidationProblemDetails : ProblemDetails
    {
        public IDictionary<string, string[]> Errors { get; private set; }

        public ValidationProblemDetails(Dictionary<string, string[]> errors)
        {
            Status = StatusCodes.Status400BadRequest;
            Title = "One or more validation errors occurred.";
            Errors = errors;
        }
    }

}
