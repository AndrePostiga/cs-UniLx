using UniLx.Shared.Abstractions;

namespace UniLx.Domain.Entities
{
    public abstract class Entity
    {
        public string Id { get; protected set; }
        public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; protected set; } = DateTime.UtcNow;
        
        private List<Event>? _events;
        public IReadOnlyList<Event>? Events => _events;

        protected static string ProduceExternalId(string prefix) => $"{prefix}{GenerateHash()}";
        private static string GenerateHash() => Ulid.NewUlid().ToString();

        protected Entity(string id)
        {
            Id = id;
        }

        protected Entity() { }

        public void RaiseEvent(Event eventItem)
        {
            _events ??= new List<Event>();
            _events.Add(eventItem);
        }
    }
}
