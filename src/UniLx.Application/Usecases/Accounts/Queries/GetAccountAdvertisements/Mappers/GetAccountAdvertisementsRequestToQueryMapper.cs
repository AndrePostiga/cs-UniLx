using UniLx.Application.Usecases.Accounts.Queries.GetAccountAdvertisements.Models;

namespace UniLx.Application.Usecases.Accounts.Queries.GetAccountAdvertisements.Mappers
{
    public static class GetAccountAdvertisementsRequestToQueryMapper
    {
        public static GetAccountAdvertisementsQuery ToQuery(this GetAccountAdvertisementsRequest source, string accountId)
            => new(
                accountId: accountId,
                type: source.Type,
                categoryName: source.CategoryName,
                sortAsc: source.SortAsc,
                status: source.Status,
                includeExpired: source.IncludeExpired,
                createdSince: source.CreatedSince,
                createdUntil: source.CreatedUntil,
                page: source.Page ?? 1,
                pageSize: source.PageSize ?? 30);
    }
}
