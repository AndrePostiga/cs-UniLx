namespace UniLx.Domain.Entities.AdvertisementAgg.Events
{
    public class CreateAdvertisementEventModel
    {
        public string AdvertisementId { get; }
        public string OwnerId { get; }
        public string OwnerName { get; }
        public string CategoryId { get; }
        public string CategoryName { get; }
        public string Status { get; }
        public string Type { get; }
        public DateTime ExpiresAt { get; }

        public CreateAdvertisementEventModel(Advertisement advertisement)
        {
            AdvertisementId = advertisement.Id;
            OwnerId = advertisement.OwnerId;
            OwnerName = advertisement.OwnerName;
            CategoryId = advertisement.CategoryId;
            CategoryName = advertisement.CategoryName;
            Status = advertisement.Status.Name;
            Type = advertisement.Type.Name;
            ExpiresAt = advertisement.ExpiresAt;
        }
    }
}
