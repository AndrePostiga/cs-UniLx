using Marten.Linq.MatchesSql;
using NetTopologySuite.Geometries;
using System.Linq.Expressions;
using UniLx.Domain.Entities.AdvertisementAgg;
using UniLx.Domain.Entities.AdvertisementAgg.Enumerations;
using UniLx.Infra.Data.Database;
using UniLx.Shared.LibExtensions;

namespace UniLx.Application.Usecases.Advertisements.Queries.GetAdvertisement.Spec
{
    internal static class AdvertisementsSpec
    {
        public static Expression<Func<Advertisement, bool>> ToSpec(this GetAdvertisementsQuery query) 
        {
            Expression<Func<Advertisement, bool>> expression = ad => ad.Status.HasSmartEnumValue(AdvertisementStatus.Created);

            if (!string.IsNullOrWhiteSpace(query.CategoryName) && string.IsNullOrWhiteSpace(query.Type))
            {
                expression = expression.And(c => c.CategoryName == query.CategoryName!.Trim());
            }

            if (!string.IsNullOrWhiteSpace(query.Type)
                && string.IsNullOrWhiteSpace(query.CategoryName)
                && AdvertisementType.TryFromName(query.Type, true, out var type))
            {
                var localtype = type;
                expression = expression.And(ad => ad.Type.HasSmartEnumValue(localtype));
            }

            if (query.HasGeolocation && query.Geopoint is Point point)
            {
                double radiusInMeters = ConvertRadiusFromKmToMeters(query.RadiusInKm);
                expression = expression.And(x => x.MatchesSql(CustomQueries.FindNearestAdvertisements, point.X, point.Y, radiusInMeters));
            }

            return expression;
        }

        private static Expression<Func<T, bool>> CombineExpressions<T>(
            Expression<Func<T, bool>> first,
            Expression<Func<T, bool>> second)
        {
            var parameter = Expression.Parameter(typeof(T));

            // Replace parameters in the second expression with the parameter of the first expression
            var body = Expression.AndAlso(
                Expression.Invoke(first, parameter),
                Expression.Invoke(second, parameter)
            );

            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        private static double ConvertRadiusFromKmToMeters(double? radiusInKm)
            => radiusInKm is null ? 10 * 1000 : radiusInKm.Value * 1000;
    }
}
