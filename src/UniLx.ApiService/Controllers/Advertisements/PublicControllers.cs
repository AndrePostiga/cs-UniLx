using Carter;
using Carter.OpenApi;
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
            var advertisementsGroup = app.MapGroup("/advertisements")
                                         .WithTags("Public Advertisements")
                                         .IncludeInOpenApi();

            advertisementsGroup.MapGet("/{id}", PublicControllerHandlers.GetById)
                               .WithName(nameof(PublicControllerHandlers.GetById));

            advertisementsGroup.MapGet("/", PublicControllerHandlers.GetAll)
                               .WithName(nameof(PublicControllerHandlers.GetAll));
        }
    }

    internal static class PublicControllerHandlers
    {
        /// <summary>
        /// Retrieves a list of advertisements based on specified filters.
        /// </summary>
        /// <remarks>
        /// Use this endpoint to retrieve all advertisements with optional filters for search and pagination.
        /// </remarks>
        /// <param name="context">The HTTP context for the current request.</param>
        /// <param name="request">The advertisement search and filter parameters.</param>
        /// <param name="mediator">The mediator service for handling the query.</param>
        /// <param name="ct">Cancellation token for the request.</param>
        /// <returns>A list of advertisements matching the provided filters.</returns>
        internal static async Task<IResult> GetAll(HttpContext context,
                [AsParameters] GetAdvertisementsRequest request,
                [FromServices] IMediator mediator,
                CancellationToken ct)
        {
            var command = request.ToQuery();
            var response = await mediator.Send(command, ct);
            return response!;
        }

        /// <summary>
        /// Retrieves a single advertisement by its ID.
        /// </summary>
        /// <param name="context">The HTTP context for the current request.</param>
        /// <param name="id">The unique identifier for the advertisement.</param>
        /// <param name="mediator">The mediator service for handling the query.</param>
        /// <param name="ct">Cancellation token for the request.</param>
        /// <returns>The details of the specified advertisement.</returns>
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
