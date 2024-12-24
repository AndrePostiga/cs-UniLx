using Marten;
using System.Linq.Expressions;
using UniLx.Domain.Data;
using UniLx.Domain.Entities;
using IUnitOfWork = UniLx.Domain.Data.IUnitOfWork;

namespace UniLx.Infra.Data.Database.Repository
{

    internal class Repository<T>(IMartenContext martenContext, IUnitOfWork unitOfWork) : IRepository<T> where T : Entity
    {
        protected readonly IMartenContext _martenContext = martenContext ?? throw new ArgumentNullException(nameof(martenContext));
        protected readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        public IUnitOfWork UnitOfWork => _unitOfWork;        

        public async Task<Tuple<IEnumerable<T>?, int>> FindAll(int skip, int limit, bool sortAsc, Expression<Func<T, bool>> expression, CancellationToken ct)
        {
            using var session = _martenContext.QuerySession();
            var query = session
                .Query<T>()
                .Where(expression);
            
            query = sortAsc ? query.OrderBy(e => e.CreatedAt) : query.OrderByDescending(e => e.CreatedAt);

            int calculatedSkip = (skip - 1) * limit;
            var result = await query.Skip(calculatedSkip).Take(limit).ToListAsync(ct);
            var total = await query.CountAsync(ct);
            return Tuple.Create((IEnumerable<T>?)result, total);
        }

        public async Task<T?> FindOne(Expression<Func<T, bool>> expression, CancellationToken ct)
        {
            using var session = _martenContext.QuerySession();
            return await session
                    .Query<T>()
                    .Where(expression)
                    .FirstOrDefaultAsync(token: ct);
        }

        public async Task<T?> FindOneWithIncludes<TInclude>(
           Expression<Func<T, bool>> expression,
           Expression<Func<T, object>> includeExpression,
           Action<TInclude> includeAction,
           CancellationToken ct) where TInclude : notnull
        {
            using var session = _martenContext.QuerySession();
            var result = await session
                .Query<T>()
                .Include<TInclude>(includeExpression, includeAction)
                .Where(expression)                
                .FirstOrDefaultAsync(token: ct);

            return result;
        }


        public void InsertOne(T entity)
        {
            Action<IDatabaseSession> insertCommand = (session) => session.Insert(entity);
            _unitOfWork.AddCommand(insertCommand);
        }

        public void CustomSql(string sql, params object[] objects)
        {
            Action<IDatabaseSession> insertCommand = (session) => session.ExecuteSql(sql, objects);            
            _unitOfWork.AddCommand(insertCommand);
        }

        public void UpdateOne(T entity)
        {
            Action<IDatabaseSession> updateCommand = (session) => session.Update(entity);
            _unitOfWork.AddCommand(updateCommand);
        }

        public async Task<Tuple<IEnumerable<T>?, int>> FindAllWithInclude<TInclude>(int skip, int limit, bool sortAsc, bool? sortUpdatedAtAsc, Expression<Func<T, bool>> expression, Expression<Func<T, object>> includeExpression, Dictionary<string, TInclude> includeAction, CancellationToken ct) where TInclude : notnull
        {
            using var session = _martenContext.QuerySession();
            var query = session
                .Query<T>()
                .Include(includeAction).On(includeExpression)
                .Where(expression);

            query = sortAsc ? query.OrderBy(e => e.CreatedAt) : query.OrderByDescending(e => e.CreatedAt);
            query = sortUpdatedAtAsc.HasValue && sortUpdatedAtAsc.Value ? query.OrderBy(e => e.UpdatedAt) : query.OrderByDescending(e => e.UpdatedAt);

            int calculatedSkip = (skip - 1) * limit;
            var result = await query.Skip(calculatedSkip).Take(limit).ToListAsync(ct);
            var total = await query.CountAsync(ct);
            return Tuple.Create((IEnumerable<T>?)result, total);
        }
    }
}
