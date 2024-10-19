namespace UniLx.Domain.Data
{
    public interface IUnitOfWork
    {
        void AddCommand(Action<IDatabaseSession> command);
        Task Commit(CancellationToken cancellationToken = default);
    }
}
