namespace UniLx.Application.Usecases.ChatRooms.Queries.GetChatRooms.Models
{
    public class GetChatRoomsResponse
    {
        public string ChatRoomId { get; set; }
        public string AdvertisementId { get; set; }
        public string SenderId { get; set; }
        public GetChatRoomsResponseAdvertisement Advertisement { get; set; }
        public DateTime InitiatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
    }

    public class GetChatRoomsResponseAdvertisement
    {
        public string Title { get; set; }
        public int? Price { get; set; }
        public string Type { get; set; }
        public string CategoryName { get; set; }
        public GetChatRoomsResponseOwner Owner { get; set; }
        public DateTime ExpiresAt { get; set; }
    }

    public class GetChatRoomsResponseOwner
    {
        public string OwnerId { get; set; }
        public string OwnerName { get; set; }
    }
}
