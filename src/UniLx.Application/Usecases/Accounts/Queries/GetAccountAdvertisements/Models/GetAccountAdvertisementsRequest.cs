using Microsoft.AspNetCore.Mvc;

namespace UniLx.Application.Usecases.Accounts.Queries.GetAccountAdvertisements.Models
{
    public class GetAccountAdvertisementsRequest
    {
        public bool? SortAsc { get; set; }
        public string? Type { get; set; }
        public string? CategoryName { get; set; }
        public string? Status { get; set; }
        public bool? IncludeExpired { get; set; }
        public DateTime? CreatedSince { get; set; }
        public DateTime? CreatedUntil { get; set; }

        public int? Page { get; set; }
        public int? PageSize { get; set; }
    }
}
