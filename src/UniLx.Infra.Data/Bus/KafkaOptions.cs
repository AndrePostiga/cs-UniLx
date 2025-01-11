namespace UniLx.Infra.Data.Bus
{
    public class KafkaOptions
    {
        public const string Section = "Kafka";

        public string BootstrapServers { get; set; }
        public string ClientId { get; set; }
        public int RetryBackoffMaxMs { get; set; }
        public int MessageSendMaxRetries { get; set; }
        public int MessageTimeoutMs { get; set; }
        public int BatchNumMessages { get; set; }
        public int LingerMs { get; set; }

        public TopicOptions CreateAdvertisementTopic { get; set; } = new();

        public class TopicOptions
        {
            public string Name { get; set; }
        }
    }
}
