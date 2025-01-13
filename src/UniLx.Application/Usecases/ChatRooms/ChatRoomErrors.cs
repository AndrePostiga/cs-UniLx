using UniLx.Shared.Abstractions;

namespace UniLx.Application.Usecases.ChatRooms
{
    public static class ChatRoomErrors
    {
        public static readonly Error NotFound = new(System.Net.HttpStatusCode.NotFound, "ChatRooms.NotFound", "Can't found chatroom with provided data.");
        public static readonly Error AccountNotFound = new(System.Net.HttpStatusCode.NotFound, "ChatRooms.AccountNotFound", "Can't found account with provided data.");
        public static readonly Error AdvertisementNotFound = new(System.Net.HttpStatusCode.NotFound, "ChatRooms.AdvertisementNotFound", "Can't found advertisement with provided data.");
        public static readonly Error AdvertisementIsExpired = new(System.Net.HttpStatusCode.PreconditionFailed, "ChatRooms.AdvertisementIsExpired", "Can't create chat room advertisement is expired.");
        public static readonly Error AdvertisementOwnerEqualToSenderId = new(System.Net.HttpStatusCode.PreconditionFailed, "ChatRooms.AdvertisementOwnerEqualToSenderId", "Can't create chat room advertisement owner is equal to sender id.");
        public static readonly Error SenderIdNotFound = new(System.Net.HttpStatusCode.NotFound, "ChatRooms.NotFound", "Can't found sender with provided data.");
        public static readonly Error NotOwner = new(System.Net.HttpStatusCode.Forbidden, "ChatRooms.NotOwner", "Can't retrieve, you are not owner of requested data.");
        public static readonly Error NotAuthorized = new(System.Net.HttpStatusCode.Forbidden, "ChatRooms.NotAuthorized", "Can't retrieve, you are not authorized to get data.");
        public static readonly Error Conflict = new(System.Net.HttpStatusCode.Conflict, "ChatRooms.Conflict", "Chat Room is already created for this data combination.");
    }
}
