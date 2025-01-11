using UniLx.Shared.Abstractions;

namespace UniLx.Domain.Data
{
    public interface IUnitOfWork
    {
        void AddCommand(Action<IDatabaseSession> command);
        void AddEvents(List<Event>? events);
        Task Commit(CancellationToken cancellationToken = default);
    }
}
