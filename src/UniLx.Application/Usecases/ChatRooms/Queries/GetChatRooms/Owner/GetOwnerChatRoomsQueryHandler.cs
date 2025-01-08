using Microsoft.AspNetCore.Http;
using UniLx.Application.Usecases.ChatRooms.Queries.GetChatRooms.Mappers;
using UniLx.Application.Usecases.ChatRooms.Queries.GetChatRooms.Models;
using UniLx.Domain.Data;
using UniLx.Domain.Entities.AdvertisementAgg;
using UniLx.Shared.Abstractions;

namespace UniLx.Application.Usecases.ChatRooms.Queries.GetChatRooms.Owner
{
    internal class GetOwnerChatRoomsQueryHandler : IQueryHandler<GetOwnerChatRoomsQuery, IResult>
    {
        private readonly IChatRoomRepository _chatRoomRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IRequestContext _requestContext;

        public GetOwnerChatRoomsQueryHandler(
            IChatRoomRepository chatRoomRepository,
            IAccountRepository accountRepository,
            IRequestContext requestContext)
        {
            _chatRoomRepository = chatRoomRepository;
            _accountRepository = accountRepository;
            _requestContext = requestContext;
        }

        public async Task<IResult> Handle(GetOwnerChatRoomsQuery request, CancellationToken cancellationToken)
        {
            if (request.OwnerId != _requestContext.AccountId)
                return ChatRoomErrors.NotOwner.ToBadRequest();

            var account = await _accountRepository.FindOne(x => x.Id == request.OwnerId, cancellationToken);
            if (account == null)
                return ChatRoomErrors.AccountNotFound.ToBadRequest();

            var includes = new Dictionary<string, Advertisement>();
            var (chatRooms, count) = await _chatRoomRepository.FindAllWithInclude(
                skip: request.Page,
                limit: request.PageSize,
                sortAsc: false,
                sortUpdatedAtAsc: false,
                x => x.AdvertisementOwnerId == request.OwnerId,
                x => x.AdvertisementId,
                includes,
                cancellationToken);

            if (chatRooms == null)
                return ChatRoomErrors.NotFound.ToBadRequest();

            var activeChatRooms = chatRooms
                .Where(chatRoom =>
                    includes.TryGetValue(chatRoom.AdvertisementId, out var advertisement) &&
                    advertisement.IsActive())
                .ToList();

            var response = activeChatRooms!.Select(c => c.ToResponse(includes[c.AdvertisementId]));
            var result = PaginatedQueryResponse<GetChatRoomsResponse>.WithContent(response, request.Page, request.PageSize, count);
            return Results.Ok(result);
        }
    }
}
