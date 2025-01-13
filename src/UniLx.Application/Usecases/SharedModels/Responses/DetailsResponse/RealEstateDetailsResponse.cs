namespace UniLx.Application.Usecases.SharedModels.Responses.DetailsResponse
{
    public class RealEstateDetailsResponse : BaseDetailsResponse
    {
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
