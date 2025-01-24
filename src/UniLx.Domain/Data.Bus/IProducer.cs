using UniLx.Shared.Abstractions;

namespace UniLx.Domain.Data.Bus
{
    public interface IProducer
    {
        public Task Produce(Event notification, string type, DateTime dispatchAt, CancellationToken cancellationToken);
    }
}
