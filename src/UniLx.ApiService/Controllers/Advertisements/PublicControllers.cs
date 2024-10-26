using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UniLx.Application.Usecases.Advertisements.Queries.GetAdvertisement.Mappers;
using UniLx.Application.Usecases.Advertisements.Queries.GetAdvertisement.Models;
using UniLx.Application.Usecases.Advertisements.Queries.GetAdvertisementById;

namespace UniLx.ApiService.Controllers.Advertisements
{
    public class PublicControllers : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/advertisements/{id}", PublicControllerHandlers.GetById);
            app.MapGet("/advertisements", PublicControllerHandlers.GetAll);
        }
    }

    internal static class PublicControllerHandlers
    {

        internal static async Task<IResult> GetAll(HttpContext context,
                [AsParameters] GetAdvertisementsRequest request,
                [FromServices] IMediator mediator,
                CancellationToken ct)
        {
            var command = request.ToQuery();
            var response = await mediator.Send(command, ct);
            return response!;
        }

        internal static async Task<IResult> GetById(HttpContext context,
                string id,
                [FromServices] IMediator mediator,
                CancellationToken ct)
        {
            var command = new GetAdvertisementByIdQuery(id);
            var response = await mediator.Send(command, ct);
            return response!;
        }
    }
}
