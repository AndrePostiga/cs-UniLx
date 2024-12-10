using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Supabase.Storage.Exceptions;

namespace UniLx.ApiService.ExceptionHandlers
{
    public sealed class SupabaseExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<SupabaseExceptionHandler> _logger;

        public SupabaseExceptionHandler(ILogger<SupabaseExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext, 
            Exception exception, 
            CancellationToken cancellationToken)
        {
            if (exception is not SupabaseStorageException)
                return false;

            var supabaseStorageException = exception as SupabaseStorageException;

            _logger.LogError(
                supabaseStorageException,
                "Exception occurred: {Message}",
                supabaseStorageException!.Message);

            var problemDetails = new ProblemDetails
            {
                Status = supabaseStorageException.StatusCode,                
                Title = supabaseStorageException.Reason.ToString(),      
                Type = nameof(SupabaseStorageException),
                Detail = supabaseStorageException.Message
            };

            httpContext.Response.StatusCode = problemDetails.Status.Value;

            await httpContext.Response
                .WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}
