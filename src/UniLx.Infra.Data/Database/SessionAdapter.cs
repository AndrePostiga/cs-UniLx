using Marten;
using UniLx.Domain.Data;
using UniLx.Domain.Entities;

namespace UniLx.Infra.Data.Database
{
    internal class SessionAdapter(IDocumentSession documentSession) : IDatabaseSession
    {
        private readonly IDocumentSession _documentSession = documentSession ?? throw new ArgumentNullException(nameof(documentSession));

        public static SessionAdapter AdaptToDatabaseSession(IDocumentSession documentSession) => new(documentSession);

        public void Insert<T>(T entity) where T : Entity
        {
            ArgumentNullException.ThrowIfNull(entity);
            _documentSession.Insert(entity);
        }
        
        public void Store<T>(T entity) where T : Entity
        {
            ArgumentNullException.ThrowIfNull(entity);
            _documentSession.Store(entity);
        }

        public void Update<T>(T entity) where T : Entity
        {
            ArgumentNullException.ThrowIfNull(entity);
            _documentSession.Update(entity);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _documentSession.SaveChangesAsync(cancellationToken);
        }
    }
}
