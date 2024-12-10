using System.Linq.Expressions;
using UniLx.Domain.Entities;

namespace UniLx.Domain.Data
{
    public interface IRepository<T> where T : Entity
    {
        Task<T?> FindOne(Expression<Func<T, bool>> expression, CancellationToken ct);
        Task<Tuple<IEnumerable<T>?, int>> FindAll(int skip, int limit, bool sortAsc, Expression<Func<T, bool>> expression, CancellationToken ct);
        void UpdateOne(T entity);
        void InsertOne(T entity);
        void CustomSql(string sql, params object[] objects);
        IUnitOfWork UnitOfWork {  get; }
    }
}
