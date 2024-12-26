using Carter;
using Carter.OpenApi;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using UniLx.Application.Usecases.Advertisements.Commands.AdvertisementGeneratePresignUrl;
using UniLx.Application.Usecases.Advertisements.Commands.CreateAdvertisement.Mappers;
using UniLx.Application.Usecases.Advertisements.Commands.CreateAdvertisement.Models.Request;
using UniLx.Shared.Abstractions;

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
                .WithName(nameof(AdminControllerHandlers.CreateAdvertisement))
                .RequireAuthorization(new AllowedGroups(Groups.User));

            advertisementsGroup
                .MapPatch("{advertisementId}/presign-url/{filename}", AdminControllerHandlers.GeneratePresignUrl)
                .WithName(nameof(AdminControllerHandlers.GeneratePresignUrl))
                .RequireAuthorization(new AllowedGroups(Groups.User));
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

        /// <summary>
        /// Generates pre-signed URLs for uploading files to S3.
        /// </summary>
        /// <remarks>
        /// This endpoint generates pre-signed URLs to allow direct uploads to S3 for files associated with a specific advertisement. 
        /// The action can be performed on behalf of another user if an impersonation header ("X-Impersonate") is provided.
        /// </remarks>
        /// <param name="context">The HTTP context of the current request.</param>
        /// <param name="advertisementId">The unique identifier of the advertisement.</param>
        /// <param name="filename">The name of the image.</param>
        /// <param name="impersonatedUser">The user to impersonate for this action, provided in the "X-Impersonate" header.</param>
        /// <param name="mediator">The mediator service responsible for handling the command.</param>
        /// <param name="ct">A cancellation token for the operation.</param>
        /// <returns>A result containing the generated pre-signed URLs or an indication of failure.</returns>

        internal static async Task<IResult> GeneratePresignUrl(HttpContext context,
                string advertisementId,
                string filename,
                [FromHeader(Name = Constants.AccountImpersonateKey)] string impersonatedUser,
                [FromServices] IMediator mediator,
                CancellationToken ct)
        {
            var command = new AdvertisementGeneratePresignUrlCommand(advertisementId, filename, impersonatedUser);
            var response = await mediator.Send(command, ct);
            return response!;
        }
    }
}
