namespace UniLx.Application.Usecases.Advertisements.Commands.CreateAdvertisement.Models.Request.DetailsRequest
{
    public class BeautyDetailsRequest
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int? Price { get; set; }
        public string? ProductType { get; set; }
        public string? Brand { get; set; }
        public string? SkinType { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public List<string>? Ingredients { get; set; }
        public bool? IsOrganic { get; set; }
    }
}