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

        public async Task<Tuple<IEnumerable<Advertisement>?, int>> FindNearestLocation(int skip, int limit, bool sortAsc, 
            Expression<Func<Advertisement, bool>> expression,
            Geometry geopoint,
            double? radiusInKm,
            CancellationToken ct)
        {
            using var session = _martenContext.QuerySession();

            // Start with the initial expression filter
            var query = session.Query<Advertisement>().Where(expression);

            // Add location-based filtering if Geopoint and Radius are specified
            if (geopoint is Point point)
            {
                var radiusInMeters = radiusInKm is null ? 10* 1000 : radiusInKm.Value * 1000; // Convert radius from km to meters
                query = query.Where(x => x.MatchesSql(CustomQueries.FindNearestAdvertisements, point.X, point.Y, radiusInMeters));
            }

            // Apply sorting
            query = sortAsc ? query.OrderBy(e => e.Id) : query.OrderByDescending(e => e.Id);

            int calculatedSkip = (skip - 1) * limit;

            var result = await query.Skip(calculatedSkip).Take(limit).ToListAsync(ct);
            var total = await query.CountAsync(ct);
            return Tuple.Create((IEnumerable<Advertisement>?)result, total);
        }
    }
}
