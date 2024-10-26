namespace UniLx.Application.Usecases.Advertisements.Commands.CreateAdvertisement.Models.Request
{
    public class AddressRequest
    {
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string? Country { get; set; }
        public string? State { get; set; }
        public string? City { get; set; }
        public string? Neighborhood { get; set; }
        public string? ZipCode { get; set; }
        public string? Street { get; set; }
        public string? Number { get; set; }
        public string? Complement { get; set; }
    }
}
