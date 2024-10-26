namespace UniLx.Application.Usecases.Advertisements.SharedModels.Responses.DetailsResponse
{
    internal class BeautyDetailsResponse : BaseDetailsResponse
    {
        public string? Brand { get; set; }
        public string? SkinType { get; set; }
        public bool? IsOrganic { get; set; }
        public string? ProductType { get; set; }
        public IEnumerable<string>? Ingredients { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }
}
