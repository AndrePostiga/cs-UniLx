namespace UniLx.Application.Usecases.Accounts.Queries.GetAccountInterestedAdvertisements.Models
{
    public class GetAccountInterestedAdvertisementsRequest
    {
        public bool? SortAsc { get; set; }
        public string? Status { get; set; }
        public bool? IncludeExpired { get; set; }
        public DateTime? CreatedSince { get; set; }
        public DateTime? CreatedUntil { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }
    }
}
