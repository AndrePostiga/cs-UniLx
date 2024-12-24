using Carter;
using Carter.OpenApi;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using UniLx.Application.Usecases.ChatRooms.Commands.CreateChatRoom.Mappers;
using UniLx.Application.Usecases.ChatRooms.Commands.CreateChatRoom.Models;
using UniLx.Application.Usecases.ChatRooms.Queries.GetChatRooms.Mappers;
using UniLx.Application.Usecases.ChatRooms.Queries.GetChatRooms.Models;
using UniLx.Shared.Abstractions;

namespace UniLx.ApiService.Controllers.ChatRooms
{

    [ExcludeFromCodeCoverage]
    public class ChatRoomsControllers : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var chatAdminGroup = app.MapGroup("/chats")
                                 .WithTags("Admin chats")
                                 .IncludeInOpenApi();

            chatAdminGroup.MapGet("/my-advertisements", AdminControllerHandlers.GetOwnerChatRooms)
                  .WithName(nameof(AdminControllerHandlers.GetOwnerChatRooms))
                  .RequireAuthorization(new AllowedGroups(Groups.User));

            chatAdminGroup.MapGet("/my-chats", AdminControllerHandlers.GetUserChatRooms)
                  .WithName(nameof(AdminControllerHandlers.GetUserChatRooms))
                  .RequireAuthorization(new AllowedGroups(Groups.User));

            chatAdminGroup.MapPost("/", AdminControllerHandlers.CreateChatRoom)
                  .WithName(nameof(AdminControllerHandlers.CreateChatRoom))
                  .RequireAuthorization(new AllowedGroups(Groups.User));
        }
    }

    public static class AdminControllerHandlers
    {   
        internal static async Task<IResult> CreateChatRoom([FromBody] CreateChatRoomRequest request,
                [FromServices] IMediator mediator,
                CancellationToken ct)
        {
            var command = request.ToCommand();
            var response = await mediator.Send(command, ct);
            return response!;
        }

        internal static async Task<IResult> GetUserChatRooms(HttpContext context,
                [AsParameters] GetChatRoomsRequest request,
                [FromServices] IMediator mediator,
                CancellationToken ct)
        {
            var command = request.ToUserQuery();
            var response = await mediator.Send(command, ct);
            return response!;
        }

        internal static async Task<IResult> GetOwnerChatRooms(HttpContext context,
                [AsParameters] GetChatRoomsRequest request,
                [FromServices] IMediator mediator,
                CancellationToken ct)
        {
            var command = request.ToOwnerQuery();
            var response = await mediator.Send(command, ct);
            return response!;
        }
    }
}
