using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UniLx.Application.Shared.UseCases.CreatePresignedImage;
using UniLx.Application.Usecases.Accounts.Commands.CreateAccount;
using UniLx.Application.Usecases.Accounts.Mappers;
using UniLx.Application.Usecases.Accounts.Requests;

namespace UniLx.ApiService.Controllers.Accounts
{
    public class AdminControllers : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/accounts", AdminControllerHandlers.CreateAccount);
            app.MapGet("/accounts/profile-picture-sign", AdminControllerHandlers.CreateAvatarImage);
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

        internal static async Task<IResult> CreateAvatarImage(
                [FromServices] IMediator mediator,
                CancellationToken ct)
        {
            var command = new CreatePresignedImageCommand();
            var response = await mediator.Send(command, ct);
            return response!;
        }
    }
}
