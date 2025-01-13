using UniLx.Application.Usecases.Accounts.Queries.GetAccountInterestedAdvertisements.Models;

namespace UniLx.Application.Usecases.Accounts.Queries.GetAccountInterestedAdvertisements.Mappers
{
    public static class GetAccountInterestedAdvertisementsRequestToQueryMapper
    {
        public static GetAccountInterestedAdvertisementsQuery ToQuery(this GetAccountInterestedAdvertisementsRequest source, string accountId)
            => new(
                accountId: accountId,
                sortAsc: source.SortAsc,
                status: source.Status,
                includeExpired: source.IncludeExpired,
                createdSince: source.CreatedSince,
                createdUntil: source.CreatedUntil,
                page: source.Page ?? 1,
                pageSize: source.PageSize ?? 30);
    }
}
