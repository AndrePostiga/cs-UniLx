using Microsoft.AspNetCore.Http;
using UniLx.Shared.Abstractions;

namespace UniLx.Application.Usecases.ChatRooms.Queries.GetChatRooms.User
{
    public class GetUserChatRoomsQuery : IQuery<IResult>
    {
        public string SenderId { get; private set; }

        public int Page { get; private set; }
        public int PageSize { get; private set; }

        public GetUserChatRoomsQuery(string? senderId, int? page, int? pageSize)
        {
            SenderId = senderId;
            Page = page ?? 1;
            PageSize = pageSize ?? 10;
        }
    }
}
