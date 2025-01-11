using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text;
using UniLx.Domain.Data.Bus;
using UniLx.Shared.Abstractions;

namespace UniLx.Infra.Data.Bus.Services
{
    public class CreateAdvertisementProducer : IProducer
    {
        private readonly IProducer<Null, object> _producer;
        private readonly ILogger<CreateAdvertisementProducer> _logger;
        private readonly IOptions<KafkaOptions> _options;

        public CreateAdvertisementProducer(IProducer<Null, object> producer, IOptions<KafkaOptions> options, ILogger<CreateAdvertisementProducer> logger)
        {
            _producer = producer;
            _logger = logger;
            _options = options;
        }

        public async Task Produce(Event notification, string type, DateTime dispatchAt, CancellationToken cancellationToken)
        {
            var messageNotification = notification as Domain.Entities.AdvertisementAgg.Events.CreateAdvertisementEvent;
            if (messageNotification is null)
            {
                _logger.LogWarning("Notification is null or not of the expected type.");
                return;
            }

            var message = new Message<Null, object>()
            {
                Value = messageNotification,
                Headers = new Headers
                {
                    { "type", Encoding.UTF8.GetBytes(notification.Type) },
                },
            };

            try
            {                
                var deliveryResult = await _producer.ProduceAsync(_options.Value.CreateAdvertisementTopic.Name, message, cancellationToken);
                _logger.LogInformation("Message delivered to {TopicPartitionOffset}", deliveryResult.TopicPartitionOffset);
            }
            catch (ProduceException<Null, object> ex)
            {
                _logger.LogError(exception: ex, "Error: {Reason}", ex.Error.Reason);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while producing a message.");
                throw;
            }
        }
    }
}
