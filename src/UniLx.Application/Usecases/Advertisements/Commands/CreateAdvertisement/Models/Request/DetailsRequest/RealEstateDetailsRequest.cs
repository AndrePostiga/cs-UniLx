namespace UniLx.Application.Usecases.Advertisements.Commands.CreateAdvertisement.Models.Request.DetailsRequest
{
    public class RealEstateDetailsRequest
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int? Price { get; set; }

        public double? LotSizeInSquareMeters { get; set; }
        public double? ConstructedSquareFootage { get; set; }
        public int? Bedrooms { get; set; }
        public int? Bathrooms { get; set; }
        public int? ParkingSpaces { get; set; }
        public string? PropertyType { get; set; }
        public string? Condition { get; set; }
        public int? Floors { get; set; }
        public List<string>? AdditionalFeatures { get; set; }
    }
}
