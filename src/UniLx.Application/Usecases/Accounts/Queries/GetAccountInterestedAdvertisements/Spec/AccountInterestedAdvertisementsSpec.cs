using Marten;
using System.Linq.Expressions;
using UniLx.Domain.Entities.AdvertisementAgg;
using UniLx.Domain.Entities.AdvertisementAgg.Enumerations;
using UniLx.Shared.LibExtensions;

namespace UniLx.Application.Usecases.Accounts.Queries.GetAccountInterestedAdvertisements.Spec
{
    internal static class AccountInterestedAdvertisementsSpec
    {
        internal static Expression<Func<Advertisement, bool>> ToSpec(this GetAccountInterestedAdvertisementsQuery query,
            IEnumerable<string>? interestedAdvertisements)
        {
            Expression<Func<Advertisement, bool>> expression = ad => ad.OwnerId != query.AccountId;

            if (interestedAdvertisements is not null && interestedAdvertisements.Any())
            {
                expression = expression.And(ad => ad.Id.In(interestedAdvertisements.ToArray()));
            }

            if (!string.IsNullOrWhiteSpace(query.Status) && AdvertisementStatus.TryFromName(query.Status, true, out var status))
            {
                expression = expression.And(ad => ad.Status.HasSmartEnumValue(status));

                if (query.IncludeExpired)
                    expression = expression.Or(ad => ad.Status.HasSmartEnumValue(AdvertisementStatus.Expired));
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
