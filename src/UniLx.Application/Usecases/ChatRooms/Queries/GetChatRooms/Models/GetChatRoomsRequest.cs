namespace UniLx.Application.Usecases.ChatRooms.Queries.GetChatRooms.Models
{
    public class GetChatRoomsRequest
    {
        public string? AccountId { get; set; }

        // Pagination
        public int? Page { get; set; } = 1;
        public int? PageSize { get; set; } = 10;
    }
}
