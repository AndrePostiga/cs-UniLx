using System.Linq.Expressions;
using UniLx.Domain.Entities.AdvertisementAgg;
using UniLx.Domain.Entities.AdvertisementAgg.Enumerations;
using UniLx.Shared.LibExtensions;

namespace UniLx.Application.Usecases.Accounts.Queries.GetAccountAdvertisements.Spec
{
    internal static class AccountAdvertisementSpec
    {
        public static Expression<Func<Advertisement, bool>> ToSpec(this GetAccountAdvertisementsQuery query)
        {
            Expression<Func<Advertisement, bool>> expression = ad => ad.OwnerId == query.AccountId;

            if (!string.IsNullOrWhiteSpace(query.Status) && AdvertisementStatus.TryFromName(query.Status, true, out var status))
            {
                expression = expression.And(ad => ad.Status.HasSmartEnumValue(status));

                if (query.IncludeExpired)
                    expression = expression.Or(ad => ad.Status.HasSmartEnumValue(AdvertisementStatus.Expired));
            }

            if (!string.IsNullOrWhiteSpace(query.CategoryName) && string.IsNullOrWhiteSpace(query.Type))
            {
                expression = expression.And(c => c.CategoryName == query.CategoryName!.Trim());
            }

            if (!string.IsNullOrWhiteSpace(query.Type)
                && string.IsNullOrWhiteSpace(query.CategoryName)
                && AdvertisementType.TryFromName(query.Type, true, out var type))
            {
                expression = expression.And(ad => ad.Type.HasSmartEnumValue(type));
            }

            if (query.CreatedSince.HasValue)
            {
                expression = expression.And(ad => ad.CreatedAt >= query.CreatedSince.Value.Date);
            }

            if (query.CreatedUntil.HasValue)
            {
                expression = expression.And(ad => ad.CreatedAt <= query.CreatedUntil.Value.Date);
            }

            return expression;
        }
    }
}
