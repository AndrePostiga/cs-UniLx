using System.Linq.Expressions;
using UniLx.Domain.Entities.AdvertisementAgg;
using NetTopologySuite.Geometries;

namespace UniLx.Domain.Data
{
    public interface IAdvertisementRepository : IRepository<Advertisement>
    {
        Task<Tuple<IEnumerable<Advertisement>?, int>> FindAdvertisements(int skip, int limit, bool sortAsc,
            Expression<Func<Advertisement, bool>> expression,
            Geometry geopoint,
            double? radiusInKm,
            CancellationToken ct);
    }
}
