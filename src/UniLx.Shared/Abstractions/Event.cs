using MediatR;

namespace UniLx.Shared.Abstractions
{
    public abstract class Event : INotification
    {
        public Guid MessageId { get; private set; }
        public DateTime Timestamp { get; private set; }
        public string Type { get; private set; }

        protected Event(string type)
        {
            MessageId = Guid.NewGuid();
            Timestamp = DateTime.UtcNow;
            Type = type;
        }
    }
}
