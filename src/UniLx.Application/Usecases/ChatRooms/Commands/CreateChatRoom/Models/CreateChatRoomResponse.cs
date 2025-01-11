namespace UniLx.Application.Usecases.ChatRooms.Commands.CreateChatRoom.Models
{
    public class CreateChatRoomResponse
    {
        public string ChatRoomId { get; set; }
        public string AdvertisementId { get; set; }
        public string SenderId { get; set; }
        public CreateChatRoomResponseAdvertisement Advertisement { get; set; }
        public DateTime InitiatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
    }

    public class CreateChatRoomResponseAdvertisement
    {
        public string Title { get; set; }
        public int? Price { get; set; }
        public string Type { get; set; }
        public string CategoryName { get; set; }
        public CreateChatRoomResponseOwner Owner { get; set; }
        public DateTime ExpiresAt { get; set; }
    }

    public class CreateChatRoomResponseOwner
    {
        public string OwnerId { get; set; }
        public string OwnerName { get; set; }
        public string? OwnerProfilePicture { get; set; }
    }
}
