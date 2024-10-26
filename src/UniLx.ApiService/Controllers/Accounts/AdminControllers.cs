using Carter;
using Carter.OpenApi;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UniLx.Application.Usecases.Accounts.Commands.CreateAccount.Mappers;
using UniLx.Application.Usecases.Accounts.Commands.CreateAccount.Models;
using UniLx.Application.Usecases.Accounts.Commands.UpdateProfilePicture;
using UniLx.Application.Usecases.Accounts.Commands.UpdateProfilePicture.Models;
using UniLx.Application.Usecases.Accounts.Commands.UpdateRating;
using UniLx.Application.Usecases.Accounts.Commands.UpdateRating.Models;
using UniLx.Application.Usecases.Accounts.Queries.GetAccountById;
using UniLx.Application.Usecases.Shared.CreatePresignedImage;

namespace UniLx.ApiService.Controllers.Accounts
{
    public class AdminControllers : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var adminGroup = app.MapGroup("/accounts")
                                 .WithTags("Admin Accounts")
                                 .IncludeInOpenApi();

            adminGroup.MapPost("/", AdminControllerHandlers.CreateAccount)
                  .WithName(nameof(AdminControllerHandlers.CreateAccount));

            adminGroup.MapGet("/profile-picture-sign", AdminControllerHandlers.CreateProfilePicturePresignUrl)
                  .WithName(nameof(AdminControllerHandlers.CreateProfilePicturePresignUrl));

            adminGroup.MapGet("/{id}", AdminControllerHandlers.GetAccountById)
                  .WithName(nameof(AdminControllerHandlers.GetAccountById));

            adminGroup.MapPatch("/{id}/rating", AdminControllerHandlers.UpdateRating)
                  .WithName(nameof(AdminControllerHandlers.UpdateRating));

            adminGroup.MapPatch("/{id}/profile-picture", AdminControllerHandlers.UpdateProfilePicture)
                  .WithName(nameof(AdminControllerHandlers.UpdateProfilePicture));
        }
    }

    internal static class AdminControllerHandlers
    {
        /// <summary>
        /// Creates a new account.
        /// </summary>
        /// <remarks>
        /// Use this endpoint to create a new account with all required fields.
        /// </remarks>
        /// <param name="request">The account creation request.</param>
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
        /// <param name="id">The account ID.</param>
        /// <param name="request">The rating update request.</param>
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
    }
}
