namespace UniLx.Application.Usecases.Advertisements.Commands.CreateAdvertisement.Models.Request.DetailsRequest
{
    public class OthersDetailsRequest
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int? Price { get; set; }
        public string? Condition { get; set; }   
        public string? Brand { get; set; }
        public List<string>? Features { get; set; }
        public DateTime? WarrantyUntil { get; set; }
    }
}
