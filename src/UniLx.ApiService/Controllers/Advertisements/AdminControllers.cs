using Carter;
using Carter.OpenApi;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using UniLx.Application.Usecases.Advertisements.Commands.CreateAdvertisement.Mappers;
using UniLx.Application.Usecases.Advertisements.Commands.CreateAdvertisement.Models.Request;

namespace UniLx.ApiService.Controllers.Advertisements
{
    [ExcludeFromCodeCoverage]
    public class AdminControllers : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var advertisementsGroup = app.MapGroup("/advertisements")
                                        .WithTags("Admin Advertisements")
                                        .IncludeInOpenApi();

            advertisementsGroup
                .MapPost("/", AdminControllerHandlers.CreateAdvertisement)
                .WithName(nameof(AdminControllerHandlers.CreateAdvertisement));
        }
    }

    [ExcludeFromCodeCoverage]
    internal static class AdminControllerHandlers
    {
        /// <summary>
        /// Creates a new advertisement.
        /// </summary>
        /// <remarks>
        /// Use this endpoint to create a new advertisement with the specified details.
        /// </remarks>
        /// <param name="context">The HTTP context for the current request.</param>
        /// <param name="request">The advertisement creation request.</param>
        /// <param name="impersonatedUser">The user to impersonate for this action, provided in the "X-Impersonate" header.</param>
        /// <param name="mediator">The mediator service for sending the command.</param>
        /// <param name="ct">Cancellation token for the request.</param>
        /// <returns>A result indicating success or failure.</returns>
        internal static async Task<IResult> CreateAdvertisement(HttpContext context, 
                [FromBody] CreateAdvertisementRequest request,
                [FromHeader(Name = Constants.AccountImpersonateKey)] string impersonatedUser,
                [FromServices] IMediator mediator,
                CancellationToken ct)
        {
            var command = request.ToCommand(impersonatedUser);
            var response = await mediator.Send(command, ct);
            return response!;
        }
    }
}
