using HashidsNet;

namespace UniLx.Domain.Entities
{
    public abstract class Entity(string id)
    {
        public string Id { get; protected set; } = id;
        public DateTime? CreatedAt { get; protected set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; protected set; } = DateTime.UtcNow;

        protected static string ProduceExternalId(string prefix) => $"{prefix}{GenerateHash()}";
        private static string GenerateHash()
        {
            var random = new Random();
            var hashids = new Hashids(Guid.NewGuid().ToString(), 12);
            var numbers = Enumerable.Range(0, 3).Select(r => random.Next(100)).ToList();
            return hashids.Encode(numbers);
        }
    }
}
