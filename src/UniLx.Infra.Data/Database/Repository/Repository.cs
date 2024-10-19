using Marten;
using System.Linq.Expressions;
using UniLx.Domain.Data;
using UniLx.Domain.Entities;
using IUnitOfWork = UniLx.Domain.Data.IUnitOfWork;

namespace UniLx.Infra.Data.Database.Repository
{

    internal class Repository<T>(IMartenContext martenContext, IUnitOfWork unitOfWork) : IRepository<T> where T : Entity
    {
        private readonly IMartenContext _martenContext = martenContext ?? throw new ArgumentNullException(nameof(martenContext));
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        public IUnitOfWork UnitOfWork => _unitOfWork;

        public async Task<Tuple<IEnumerable<T>?, int>> FindAll(int skip, int limit, bool sortAsc, Expression<Func<Entity, bool>> expression, CancellationToken ct)
        {
            using var session = _martenContext.QuerySession();
            var query = session.Query<T>().Where(expression);
            
            query = sortAsc ? query.OrderBy(e => e.Id) : query.OrderByDescending(e => e.Id);

            var result = await query.Skip(skip).Take(limit).ToListAsync(ct);
            var total = await query.CountAsync(ct);
            return Tuple.Create((IEnumerable<T>?)result, total);
        }

        public async Task<T?> FindOne(Expression<Func<T, bool>> expression, CancellationToken ct)
        {
            using var session = _martenContext.QuerySession();
            return await session
                .Query<T>()
                .Where(expression)
                .FirstOrDefaultAsync(ct);               
        }

        public void InsertOne(T entity)
        {
            Action<IDatabaseSession> insertCommand = (session) => session.Insert(entity);
            _unitOfWork.AddCommand(insertCommand);
        }

        public void UpdateOne(Expression<Func<T, bool>> condition, T entity, CancellationToken cancellationToken)
        {
            Action<IDatabaseSession> updateCommand = (session) => session.Update(entity);
            _unitOfWork.AddCommand(updateCommand);
        }
    }
}
