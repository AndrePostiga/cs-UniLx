using Microsoft.AspNetCore.Http;
using UniLx.Shared.Abstractions;

namespace UniLx.Application.Usecases.ChatRooms.Commands.CreateChatRoom
{
    public class CreateChatRoomCommand : ICommand<IResult>
    {
        public string AccountId { get; private set; }
        public string AdvertisementId { get; private set; }

        public CreateChatRoomCommand(string accountId, string advertisementId)
        {
            AccountId = accountId;
            AdvertisementId = advertisementId;
        }
    }
}
