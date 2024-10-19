using UniLx.Domain.Data;

namespace UniLx.Infra.Data.Database
{
    public class UnitOfWork(IMartenContext martenContext) : IUnitOfWork
    {
        private readonly IMartenContext _martenContext = martenContext ?? throw new ArgumentNullException(nameof(martenContext));
        private readonly List<Action<IDatabaseSession>> _commands = [];

        public void AddCommand(Action<IDatabaseSession> command) => _commands.Add(command);

        public async Task Commit(CancellationToken cancellationToken = default)
        {
            using var session = martenContext.OpenSession();
            var adaptedSession = SessionAdapter.AdaptToDatabaseSession(session);

            foreach (var command in _commands)
            {
                command(adaptedSession);
            }

            await adaptedSession.SaveChangesAsync(cancellationToken);            
        }
    }
}
