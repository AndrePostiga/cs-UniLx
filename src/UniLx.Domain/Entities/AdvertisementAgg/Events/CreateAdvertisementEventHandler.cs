using MediatR;
using UniLx.Domain.Data.Bus;
using UniLx.Shared.Abstractions;

namespace UniLx.Domain.Entities.AdvertisementAgg.Events
{
    internal class CreateAdvertisementEventHandler : INotificationHandler<CreateAdvertisementEvent>
    {
        private readonly IProducer _producer;
        private readonly IRequestContext _context;

        public CreateAdvertisementEventHandler(IProducer producer, IRequestContext context)
        {
            _producer = producer;
            _context = context;
        }

        public async Task Handle(CreateAdvertisementEvent notification, CancellationToken cancellationToken)
        {
            await _producer.Produce(notification, notification.Type, DateTime.UtcNow, cancellationToken);
        }
    }
}
