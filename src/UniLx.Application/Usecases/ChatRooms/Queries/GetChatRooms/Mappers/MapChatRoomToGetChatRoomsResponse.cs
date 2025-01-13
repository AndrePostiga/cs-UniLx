using UniLx.Application.Usecases.ChatRooms.Queries.GetChatRooms.Models;
using UniLx.Domain.Entities.AdvertisementAgg;
using UniLx.Domain.Entities.ChatAgg;

namespace UniLx.Application.Usecases.ChatRooms.Queries.GetChatRooms.Mappers
{
    internal static class MapChatRoomToGetChatRoomsResponse
    {
        public static GetChatRoomsResponse ToResponse(this ChatRoom chatRoom, Advertisement advertisement)
            => new()
            {
                ChatRoomId = chatRoom.Id,
                AdvertisementId = chatRoom.AdvertisementId,
                SenderId = chatRoom.SenderId,
                InitiatedAt = chatRoom.InitiatedAt,
                ExpiresAt = chatRoom.ExpiresAt,
                Advertisement = advertisement.ToResponseAdvertisement()
            };

        public static GetChatRoomsResponseAdvertisement ToResponseAdvertisement(this Advertisement advertisement)
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
