using Confluent.Kafka;
using System.Text.Json;

namespace UniLx.Infra.Data.Bus
{
    internal class Serializer<T> : ISerializer<T>
    {
        private readonly JsonSerializerOptions _options;

        public Serializer(JsonSerializerOptions options)
        {
            _options = options;
        }

        public byte[] Serialize(T data, SerializationContext context)
        {
            return JsonSerializer.SerializeToUtf8Bytes(data, _options);
        }
    }
}
