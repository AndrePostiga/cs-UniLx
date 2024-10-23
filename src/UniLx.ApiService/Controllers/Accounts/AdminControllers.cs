using Carter;
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
            app.MapPost("/accounts", AdminControllerHandlers.CreateAccount);
            app.MapGet("/accounts/profile-picture-sign", AdminControllerHandlers.CreateProfilePicturePresignUrl);
            app.MapGet("/accounts/{id}", AdminControllerHandlers.GetAccountById);
            app.MapPatch("/accounts/{id}/rating", AdminControllerHandlers.UpdateRating);
            app.MapPatch("/accounts/{id}/profile-picture", AdminControllerHandlers.UpdateProfilePicture);
        }
    }

    internal static class AdminControllerHandlers
    {
        internal static async Task<IResult> CreateAccount(
                [FromBody] CreateAccountRequest request,
                [FromServices] IMediator mediator,
                CancellationToken ct)
        {
            var command = request.ToCommand();            
            var response = await mediator.Send(command, ct);
            return response!;
        }

        internal static async Task<IResult> CreateProfilePicturePresignUrl(
                [FromServices] IMediator mediator,
                CancellationToken ct)
        {
            var command = new CreatePresignedImageCommand();
            var response = await mediator.Send(command, ct);
            return response!;
        }

        internal static async Task<IResult> GetAccountById(
                string id,
                [FromServices] IMediator mediator,
                CancellationToken ct)
        {
            var command = new GetAccountByIdQuery(id);
            var response = await mediator.Send(command, ct);
            return response!;
        }

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
