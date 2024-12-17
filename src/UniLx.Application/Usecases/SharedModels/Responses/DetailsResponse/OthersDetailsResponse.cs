namespace UniLx.Application.Usecases.SharedModels.Responses.DetailsResponse
{
    public class OthersDetailsResponse : BaseDetailsResponse
    {
        public string? Condition { get; set; }
        public string? Brand { get; set; }
        public List<string>? Features { get; set; }
        public DateTime? WarrantyUntil { get; set; }
    }
}
