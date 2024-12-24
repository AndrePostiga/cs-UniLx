using UniLx.Application.Usecases.ChatRooms.Queries.GetChatRooms.Models;
using UniLx.Application.Usecases.ChatRooms.Queries.GetChatRooms.Owner;
using UniLx.Application.Usecases.ChatRooms.Queries.GetChatRooms.User;

namespace UniLx.Application.Usecases.ChatRooms.Queries.GetChatRooms.Mappers
{
    public static class GetChatRoomsRequestToQueryMapper
    {
        public static GetOwnerChatRoomsQuery ToOwnerQuery(this GetChatRoomsRequest source)
            => new(source.AccountId, source.Page, source.PageSize);

        public static GetUserChatRoomsQuery ToUserQuery(this GetChatRoomsRequest source)
            => new(source.AccountId, source.Page, source.PageSize);
    }
}
