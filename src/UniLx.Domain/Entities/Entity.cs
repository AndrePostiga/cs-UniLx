namespace UniLx.Domain.Entities
{
    public abstract class Entity
    {
        public string Id { get; protected set; }
        public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; protected set; } = DateTime.UtcNow;

        protected static string ProduceExternalId(string prefix) => $"{prefix}{GenerateHash()}";
        private static string GenerateHash() => Ulid.NewUlid().ToString();

        protected Entity(string id)
        {
            Id = id;
        }

        protected Entity() { }
    }
}
