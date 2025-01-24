using Confluent.Kafka;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using UniLx.Domain.Data.Bus;
using UniLx.Infra.Data.Bus.Services;
using UniLx.Shared.Converters;

namespace UniLx.Infra.Data.Bus
{
    [ExcludeFromCodeCoverage]
    public static class KafkaExtensions
    {
        public static WebApplicationBuilder AddKafkaBus(this WebApplicationBuilder builder)
        {
            var configuration = builder.Configuration;
            var kafkaOptions = configuration.GetSection(KafkaOptions.Section).Get<KafkaOptions>();
            builder.Services.Configure<KafkaOptions>(builder.Configuration.GetSection(KafkaOptions.Section));

            var config = new ProducerConfig
            {
                BootstrapServers = kafkaOptions!.BootstrapServers,
                ClientId = kafkaOptions!.ClientId,
                SecurityProtocol = SecurityProtocol.Plaintext,
                Acks = Acks.Leader,
                RetryBackoffMaxMs = kafkaOptions!.RetryBackoffMaxMs,
                MessageSendMaxRetries = kafkaOptions!.MessageSendMaxRetries,
                MessageTimeoutMs = kafkaOptions!.MessageTimeoutMs,
                BatchNumMessages = kafkaOptions!.BatchNumMessages,
                LingerMs = kafkaOptions!.LingerMs,
            };

            builder.Services.AddSingleton(sp =>
            {
                var JsonSerializerOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.SnakeCaseLower,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                };

                JsonSerializerOptions.Converters.Add(new FloatConverter(2));

                var serializer = new Serializer<object>(JsonSerializerOptions);

                return new ProducerBuilder<Null, object>(config)
                    .SetValueSerializer(serializer)
                    .Build();
            });

            builder.Services.AddScoped<IProducer, CreateAdvertisementProducer>();
            return builder;
        }
    }
}
