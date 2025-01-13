using UniLx.Application.Usecases.ChatRooms.Commands.CreateChatRoom.Models;

namespace UniLx.Application.Usecases.ChatRooms.Commands.CreateChatRoom.Mappers
{
    public static class CreateChatRoomRequestToCreateChatRoomCommand
    {
        public static CreateChatRoomCommand ToCommand(this CreateChatRoomRequest source)
            => new(source.AccountId, source.AdvertisementId);
    }
}
