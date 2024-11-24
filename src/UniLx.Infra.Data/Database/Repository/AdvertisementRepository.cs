using Marten;
using Marten.Linq.MatchesSql;
using NetTopologySuite.Geometries;
using System.Linq.Expressions;
using UniLx.Domain.Data;
using UniLx.Domain.Entities.AdvertisementAgg;

namespace UniLx.Infra.Data.Database.Repository
{
    internal class AdvertisementRepository : Repository<Advertisement>, IAdvertisementRepository
    {
        public AdvertisementRepository(IMartenContext martenContext, IUnitOfWork unitOfWork) : base(martenContext, unitOfWork)
        {                    
        }

        public async Task<Tuple<IEnumerable<Advertisement>?, int>> FindAdvertisements(int skip, int limit, bool sortAsc, 
            Expression<Func<Advertisement, bool>> expression,
            Geometry geopoint,
            double? radiusInKm,
            CancellationToken ct)
        {
            using var session = _martenContext.QuerySession();
            var query = session
                .Query<Advertisement>()
                .Where(expression);

            // Apply sorting
            query = sortAsc ? query.OrderBy(e => e.Id) : query.OrderByDescending(e => e.Id);

            int calculatedSkip = (skip - 1) * limit;

            var result = await query.Skip(calculatedSkip).Take(limit).ToListAsync(ct);
            var total = await query.CountAsync(ct);
            return Tuple.Create((IEnumerable<Advertisement>?)result, total);
        }
    }
}
