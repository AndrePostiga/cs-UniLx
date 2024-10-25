using UniLx.Domain.Entities;

namespace UniLx.Domain.Data
{
    public interface IDatabaseSession
    {
        //  will perform either insertion or update depending on the existence of documents
        void Store<T>(T entity) where T : Entity;

        // stores a document only in the case that it does not already exist. Otherwise a DocumentAlreadyExistsException is thrown
        void Insert<T>(T entity) where T : Entity;

        void ExecuteSql(string sql, params object[] objects);

        // performs an update on an existing document or throws a NonExistentDocumentException in case the document cannot be found
        void Update<T>(T entity) where T : Entity;

        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
