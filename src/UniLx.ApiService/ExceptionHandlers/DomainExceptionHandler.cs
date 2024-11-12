using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Supabase.Storage.Exceptions;
using System.Net;
using UniLx.Domain.Exceptions;

namespace UniLx.ApiService.ExceptionHandlers
{
    internal sealed class DomainExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<DomainExceptionHandler> _logger;

        public DomainExceptionHandler(ILogger<DomainExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            if (exception is not DomainException)
                return false;

            var domainException = exception as DomainException;

            _logger.LogError(
                domainException,
                "Exception occurred: {Message}",
                domainException!.Message);

            var problemDetails = new ProblemDetails
            {
                Status = ((int)HttpStatusCode.PreconditionFailed),
                Title = "A domain exception occurred",
                Type = nameof(DomainException),
                Detail = domainException.Message
            };

            httpContext.Response.StatusCode = problemDetails.Status.Value;

            await httpContext.Response
                .WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}
