using UniLx.Domain.Entities.AccountAgg;
using UniLx.Domain.Entities.AdvertisementAgg;
using UniLx.Domain.Exceptions;

namespace UniLx.Domain.Entities.ChatAgg
{
    public class Message : Entity
    {
        public string Content { get; private set; }
        public string RoomId { get; private set; }
        public string MessageOwnerId { get; private set; }
        public string MessageOwnerName { get; private set; }
        public bool IsSeen { get; private set; }
        public DateTime SentAt { get; private set; }
        public DateTime? SeenAt { get; private set; }

        private Message()
        {}

        public Message(string content, ChatRoom room, Account sender) : base(ProduceExternalId("message_"))
        {
            SetContent(content);
            SetOwnership(sender);
            SetRoom(room);
            IsSeen = false;
            SentAt = DateTime.UtcNow;
        }

        private void SetRoom(ChatRoom room)
        {
            DomainException.ThrowIf(room is null, "ChatRoom cannot be null.");
            RoomId = room!.Id;            
        }

        private void SetOwnership(Account sender)
        {
            DomainException.ThrowIf(sender is null, "Sender cannot be null.");
            DomainException.ThrowIf(string.IsNullOrWhiteSpace(sender!.Id), "Sender cannot be empty.");
            MessageOwnerId = sender.Id;
            MessageOwnerName = sender.Name;
        }

        private void SetContent(string content)
        {
            DomainException.ThrowIf(string.IsNullOrWhiteSpace(content), "Cannot send null message.");
            DomainException.ThrowIf(content.Length > 500, "Cannot send messages with more than 500 characters.");
            Content = content;
        }

        public void MarkAsSeen()
        {
            if (IsSeen is true) return;
            IsSeen = true;
            SeenAt = DateTime.UtcNow;
        }
    }
}
