using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UniLx.Application.Usecases.Advertisement.Commands.CreateAdvertisement.Mappers;
using UniLx.Application.Usecases.Advertisement.Commands.CreateAdvertisement.Models;

namespace UniLx.ApiService.Controllers.Advertisements
{
    public class AdminControllers : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/advertisements", AdminControllerHandlers.CreateAdvertisement);
        }
    }

    internal static class AdminControllerHandlers
    {
        internal static async Task<IResult> CreateAdvertisement(HttpContext context, 
                [FromBody] CreateAdvertisementRequest request,
                [FromServices] IMediator mediator,
                CancellationToken ct)
        {

            string impersonatedUser = string.Empty;
            if (context.Request.Headers.TryGetValue("X-Impersonate", out var impersonateHeaderValue))
                impersonatedUser = impersonateHeaderValue.ToString();

            var command = request.ToCommand(impersonatedUser);
            var response = await mediator.Send(command, ct);
            return response!;
        }
    }
}
