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
        /// <summary>
        /// Creates a new chat room for a specified advertisement.
        /// </summary>
        /// <param name="request">The request to create a chat room, including advertisement and user details.</param>
        /// <param name="mediator">The <see cref="IMediator"/> service for handling the command.</param>
        /// <param name="ct">The <see cref="CancellationToken"/> for task cancellation.</param>
        /// <returns>An <see cref="IResult"/> indicating success or failure.</returns>
        internal static async Task<IResult> CreateChatRoom([FromBody] CreateChatRoomRequest request,
                [FromServices] IMediator mediator,
                CancellationToken ct)
        {
            var command = request.ToCommand();
            var response = await mediator.Send(command, ct);
            return response!;
        }

        /// <summary>
        /// Retrieves chat rooms initiated by the current user.
        /// </summary>
        /// <param name="context">The HTTP context of the current request.</param>
        /// <param name="request">The query parameters for retrieving chat rooms.</param>
        /// <param name="mediator">The <see cref="IMediator"/> service for handling the query.</param>
        /// <param name="ct">The <see cref="CancellationToken"/> for task cancellation.</param>
        /// <returns>An <see cref="IResult"/> containing the list of chat rooms.</returns>
        internal static async Task<IResult> GetUserChatRooms(HttpContext context,
                [AsParameters] GetChatRoomsRequest request,
                [FromServices] IMediator mediator,
                CancellationToken ct)
        {
            var command = request.ToUserQuery();
            var response = await mediator.Send(command, ct);
            return response!;
        }

        /// <summary>
        /// Retrieves chat rooms related to the user's advertisements.
        /// </summary>
        /// <param name="context">The HTTP context of the current request.</param>
        /// <param name="request">The query parameters for retrieving chat rooms related to advertisements.</param>
        /// <param name="mediator">The <see cref="IMediator"/> service for handling the query.</param>
        /// <param name="ct">The <see cref="CancellationToken"/> for task cancellation.</param>
        /// <returns>An <see cref="IResult"/> containing the list of chat rooms.</returns>
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
