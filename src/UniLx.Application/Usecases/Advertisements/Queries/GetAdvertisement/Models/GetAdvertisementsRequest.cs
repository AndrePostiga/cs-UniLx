namespace UniLx.Application.Usecases.Advertisements.Queries.GetAdvertisement.Models
{
    public class GetAdvertisementsRequest
    {
        public string? Type { get; set; }
        public string? CategoryName { get; set; }        

        // Geolocation parameters
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public double? RadiusInKm { get; set; } 

        // Pagination
        public int? Page { get; set; } = 1;

        public int? PageSize { get; set; } = 10;
    }
}
