namespace UniLx.Application.Usecases.SharedModels.Responses.DetailsResponse
{
    public class FashionDetailsResponse : BaseDetailsResponse
    {
        public string ClothingType { get; set; }
        public string Brand { get; set; }
        public List<string> Sizes { get; set; }
        public string Gender { get; set; }
        public List<string>? Colors { get; set; }
        public List<string>? Materials { get; set; }
        public List<string>? Features { get; set; }
        public string? Designer { get; set; }
        public bool? IsHandmade { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public bool? IsSustainable { get; set; }
    }
}
