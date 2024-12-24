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

        Task<T?> FindOneWithIncludes<TInclude>(
            Expression<Func<T, bool>> expression,
            Expression<Func<T, object>> includeExpression,
            Action<TInclude> includeAction,
            CancellationToken ct) where TInclude : notnull;

        Task<Tuple<IEnumerable<T>?, int>> FindAllWithInclude<TInclude>(
            int skip, int limit, bool sortAsc, bool? sortUpdatedAtAsc,
            Expression<Func<T, bool>> expression,
            Expression<Func<T, object>> includeExpression,
            Dictionary<string, TInclude> includeAction,
            CancellationToken ct) where TInclude : notnull;
    }
}
