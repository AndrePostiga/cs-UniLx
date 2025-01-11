using UniLx.Application.Usecases.ChatRooms.Commands.CreateChatRoom.Models;
using UniLx.Domain.Entities.AccountAgg;
using UniLx.Domain.Entities.AdvertisementAgg;
using UniLx.Domain.Entities.ChatAgg;

namespace UniLx.Application.Usecases.ChatRooms.Commands.CreateChatRoom.Mappers
{
    internal static class MapChatRoomToGetChatRoomsResponse
    {
        public static CreateChatRoomResponse ToResponse(this ChatRoom chatRoom, Advertisement advertisement)
            => new()
            {
                ChatRoomId = chatRoom.Id,
                AdvertisementId = chatRoom.AdvertisementId,
                SenderId = chatRoom.SenderId,
                InitiatedAt = chatRoom.InitiatedAt,
                ExpiresAt = chatRoom.ExpiresAt,
                Advertisement = advertisement.ToResponseAdvertisement()
            };

        public static CreateChatRoomResponseAdvertisement ToResponseAdvertisement(this Advertisement advertisement)
            => new()
            {
                Title = advertisement.Details.Title,
                Price = advertisement.Details.Price,
                Type = advertisement.Type.Name,
                CategoryName = advertisement.CategoryName,
                ExpiresAt = advertisement.ExpiresAt,
                Owner = new()
                {
                    OwnerId = advertisement.OwnerId,
                    OwnerName = advertisement.OwnerName
                }
            };
    }
}
