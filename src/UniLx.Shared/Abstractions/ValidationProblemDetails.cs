using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace UniLx.Shared.Abstractions
{
    public class ValidationProblemDetails : ProblemDetails
    {
        public IDictionary<string, string[]> Errors { get; private set; }

        public ValidationProblemDetails(Dictionary<string, string[]> errors)
        {
            Status = ((int)HttpStatusCode.BadRequest);
            Title = "One or more validation errors occurred.";
            Errors = errors;
        }

        public ValidationProblemDetails(Error error)
        {
            Status = ((int)error.StatusCode);
            Title = "One error occurred.";
            Errors = new Dictionary<string, string[]>
            {
                { error.Code, new string[]{ error.Description } }
            };
        }
    }
}
