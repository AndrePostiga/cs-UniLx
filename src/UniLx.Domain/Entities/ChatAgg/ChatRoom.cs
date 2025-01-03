using UniLx.Domain.Entities.AccountAgg;
using UniLx.Domain.Entities.AdvertisementAgg;
using UniLx.Domain.Exceptions;

namespace UniLx.Domain.Entities.ChatAgg
{
    public class ChatRoom : Entity
    {
        public string AdvertisementId { get; private set; }
        public string AdvertisementOwnerId { get; private set; }
        public string SenderId { get; private set; }
        public DateTime InitiatedAt { get; private set; }
        public DateTime ExpiresAt { get; private set; }
        public bool IsExpired => ExpiresAt < DateTime.UtcNow;        

        private ChatRoom()
        {}

        public ChatRoom(Account sender, Advertisement advertisement) : base(ProduceExternalId("room_"))
        {
            SetAdvertisement(advertisement);
            SetSender(sender);
            InitiatedAt = DateTime.UtcNow;
        }

        private void SetSender(Account sender)
        {
            DomainException.ThrowIf(sender is null, "InterestedUser cannot be null.");
            DomainException.ThrowIf(string.IsNullOrWhiteSpace(sender!.Id), "InterestedUserID cannot be empty.");
            SenderId = sender.Id;
            sender.AddInterest(AdvertisementId);
        }

        private void SetAdvertisement(Advertisement advertisement)
        {
            DomainException.ThrowIf(advertisement is null, "Advertisement cannot be null.");
            DomainException.ThrowIf(string.IsNullOrWhiteSpace(advertisement!.Id), "Advertisement ID cannot be empty.");
            AdvertisementId = advertisement.Id;
            AdvertisementOwnerId = advertisement.OwnerId;
            ExpiresAt = advertisement.ExpiresAt;
        }
    }
}
