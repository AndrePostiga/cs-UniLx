using Marten;

namespace UniLx.Infra.Data.Database
{
    public interface IMartenContext
    {
        IDocumentSession IdentitySession();
        IQuerySession QuerySession();
        IDocumentSession OpenSession();

    }

    public class MartenContext(IDocumentStore documentStore) : IMartenContext
    {
        private readonly IDocumentStore _documentStore = documentStore;

        public IDocumentSession IdentitySession() => _documentStore.IdentitySession(System.Data.IsolationLevel.ReadCommitted);

        public IQuerySession QuerySession() => _documentStore.QuerySession();

        // Opting for the "lightweight" session
        // option with no identity map tracking  
        public IDocumentSession OpenSession() => _documentStore.LightweightSession();
    }
}
