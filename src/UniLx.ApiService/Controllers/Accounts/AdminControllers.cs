using Carter;
using Carter.OpenApi;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using UniLx.ApiService.Authorization;
using UniLx.Application.Usecases.Accounts.Commands.CreateAccount.Mappers;
using UniLx.Application.Usecases.Accounts.Commands.CreateAccount.Models;
using UniLx.Application.Usecases.Accounts.Commands.UpdateProfilePicture;
using UniLx.Application.Usecases.Accounts.Commands.UpdateProfilePicture.Models;
using UniLx.Application.Usecases.Accounts.Commands.UpdateRating;
using UniLx.Application.Usecases.Accounts.Commands.UpdateRating.Models;
using UniLx.Application.Usecases.Accounts.Queries.GetAccountAdvertisements.Mappers;
using UniLx.Application.Usecases.Accounts.Queries.GetAccountAdvertisements.Models;
using UniLx.Application.Usecases.Accounts.Queries.GetAccountByCognitoSub;
using UniLx.Application.Usecases.Accounts.Queries.GetAccountById;
using UniLx.Application.Usecases.Shared.CreatePresignedImage;
using UniLx.Shared.Abstractions;

namespace UniLx.ApiService.Controllers.Accounts
{
    [ExcludeFromCodeCoverage]
    public class AdminControllers : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var adminGroup = app.MapGroup("/accounts")
                                 .WithTags("Admin Accounts")
                                 .IncludeInOpenApi();

            adminGroup.MapPost("/", AdminControllerHandlers.CreateAccount)
                  .WithName(nameof(AdminControllerHandlers.CreateAccount))
                  .RequireAuthorization(new AllowedGroups(Groups.User));

            adminGroup.MapGet("/profile-picture-sign", AdminControllerHandlers.CreateProfilePicturePresignUrl)
                  .WithName(nameof(AdminControllerHandlers.CreateProfilePicturePresignUrl))
                  .RequireAuthorization(new AllowedGroups(Groups.User));

            adminGroup.MapGet("/{id}", AdminControllerHandlers.GetAccountById)
                  .WithName(nameof(AdminControllerHandlers.GetAccountById))
                  .RequireAuthorization(new AllowedGroups(Groups.User));

            adminGroup.MapGet("/", AdminControllerHandlers.GetAccountByToken)
                  .WithName(nameof(AdminControllerHandlers.GetAccountByToken))
                  .RequireAuthorization(new AllowedGroups(Groups.User));

            adminGroup.MapPatch("/{id}/rating", AdminControllerHandlers.UpdateRating)
                  .WithName(nameof(AdminControllerHandlers.UpdateRating))
                  .RequireAuthorization(new AllowedGroups(Groups.User));

            adminGroup.MapPatch("/{id}/profile-picture", AdminControllerHandlers.UpdateProfilePicture)
                  .WithName(nameof(AdminControllerHandlers.UpdateProfilePicture))
                  .RequireAuthorization(new AllowedGroups(Groups.User));

            adminGroup.MapGet("/{id}/advertisements", AdminControllerHandlers.GetAccountAdvertisements)
                  .WithName(nameof(AdminControllerHandlers.GetAccountAdvertisements))
                  .RequireAuthorization(new AllowedGroups(Groups.User));
        }
    }

    [ExcludeFromCodeCoverage]
    internal static class AdminControllerHandlers
    {
        /// <summary>
        /// Creates a new account.
        /// </summary>
        /// <remarks>
        /// Use this endpoint to create a new account with all required fields.
        /// </remarks>
        /// <param name="request">The account creation request.</param>
        /// <param name="mediator"></param>
        /// <param name="ct"></param>
        /// <returns>A result indicating success or failure.</returns>
        internal static async Task<IResult> CreateAccount(
                [FromBody] CreateAccountRequest request,
                [FromServices] IMediator mediator,
                CancellationToken ct)
        {
            var command = request.ToCommand();            
            var response = await mediator.Send(command, ct);
            return response!;
        }

        /// <summary>
        /// Generates a presigned URL for profile picture upload.
        /// </summary>
        /// <remarks>
        /// This endpoint provides a presigned URL that allows clients to securely upload profile pictures.
        /// </remarks>
        internal static async Task<IResult> CreateProfilePicturePresignUrl(
                [FromServices] IMediator mediator,
                CancellationToken ct)
        {
            var command = new CreatePresignedImageCommand();
            var response = await mediator.Send(command, ct);
            return response!;
        }

        /// <summary>
        /// Gets account details by account ID.
        /// </summary>
        /// <param name="id">The account ID.</param>
        /// <param name="mediator"></param>
        /// <param name="ct"></param>
        /// <returns>The account details for the specified ID.</returns>
        internal static async Task<IResult> GetAccountById(
                string id,
                [FromServices] IMediator mediator,
                CancellationToken ct)
        {
            var command = new GetAccountByIdQuery(id);
            var response = await mediator.Send(command, ct);
            return response!;
        }

        /// <summary>
        /// Updates the rating for a specified account.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="id">The account ID.</param>
        /// <param name="request">The rating update request.</param>
        /// <param name="mediator"></param>
        /// <param name="ct"></param>
        /// <returns>A result indicating success or failure.</returns>
        internal static async Task<IResult> UpdateProfilePicture(HttpContext context,
                string id,
                [FromBody] UpdateProfilePictureRequest request,
                [FromServices] IMediator mediator,
                CancellationToken ct)
        {
            var command = new UpdateProfilePictureCommand(request.ProfilePicture, id);
            var response = await mediator.Send(command, ct);
            return response!;
        }

        /// <summary>
        /// Updates the profile picture for a specified account.
        /// </summary>
        /// <param name="id">The account ID.</param>
        /// <param name="request">The profile picture update request.</param>
        /// <param name="mediator"></param>
        /// <param name="ct"></param>
        /// <returns>A result indicating success or failure.</returns>
        internal static async Task<IResult> UpdateRating(string id,
                [FromBody] UpdateRatingRequest request,
                [FromServices] IMediator mediator,
                CancellationToken ct)
        {
            var command = new UpdateRatingCommand(request.Rating, id);
            var response = await mediator.Send(command, ct);
            return response!;
        }

        /// <summary>
        /// Retrieves a paginated list of advertisements for a specific account.
        /// </summary>
        /// <param name="id">The unique identifier of the account for which advertisements are retrieved.</param>
        /// <param name="request">The query parameters for filtering, sorting, and paginating the advertisements.</param>
        /// <param name="mediator">The mediator service used to handle the query.</param>
        /// <param name="ct">The cancellation token to observe for task cancellation.</param>
        /// <returns>
        /// A paginated list of advertisements that match the query parameters.
        /// Returns a <see cref="IResult"/> containing the advertisements or an error response if the query fails.
        /// </returns>
        internal static async Task<IResult> GetAccountAdvertisements(
                string id,
                [AsParameters] GetAccountAdvertisementsRequest request,
                [FromServices] IMediator mediator,
                CancellationToken ct)
        {
            var command = request.ToQuery(id);
            var response = await mediator.Send(command, ct);
            return response!;
        }

        /// <summary>
        /// Gets account details by access token.
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="requestContext"></param>
        /// <param name="ct"></param>
        /// <returns>The account details for the token.</returns>
        internal static async Task<IResult> GetAccountByToken([FromServices] IMediator mediator,
                [FromServices] IRequestContext requestContext,
                CancellationToken ct)
        {
            var command = new GetAccountByCognitoSubQueryExternal(requestContext.CognitoIdentifier!);
            var response = await mediator.Send(command, ct);
            return response!;
        }
    }
}
