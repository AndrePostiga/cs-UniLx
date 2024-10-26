using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UniLx.Application.Usecases.Advertisements.Commands.CreateAdvertisement.Mappers;
using UniLx.Application.Usecases.Advertisements.Commands.CreateAdvertisement.Models.Request;

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
                [FromHeader(Name = "X-Impersonate")] string impersonatedUser,
                [FromServices] IMediator mediator,
                CancellationToken ct)
        {
            var command = request.ToCommand(impersonatedUser);
            var response = await mediator.Send(command, ct);
            return response!;
        }
    }
}
