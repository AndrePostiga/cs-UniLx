using Microsoft.AspNetCore.Http;
using UniLx.Shared.Abstractions;

namespace UniLx.Application.Usecases.ChatRooms.Queries.GetChatRooms.Owner
{
    public class GetOwnerChatRoomsQuery : IQuery<IResult>
    {
        public string OwnerId { get; private set; }

        public int Page { get; private set; }
        public int PageSize { get; private set; }

        public GetOwnerChatRoomsQuery(string? ownerId, int? page, int? pageSize)
        {
            OwnerId = ownerId;
            Page = page ?? 1;
            PageSize = pageSize ?? 10;
        }
    }
}
