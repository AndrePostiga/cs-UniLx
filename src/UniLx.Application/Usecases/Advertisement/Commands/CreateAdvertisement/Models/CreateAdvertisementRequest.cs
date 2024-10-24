namespace UniLx.Application.Usecases.Advertisement.Commands.CreateAdvertisement.Models
{
    public class CreateAdvertisementRequest
    {
        public string? Type { get; set; }
        public string? SubCategory { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public AddressRequest? Address { get; set; }

        public BeautyDetailsRequest? BeautyDetails { get; set; }
        public ElectronicsDetailsRequest? ElectronicsDetails { get; set; }
        public EventDetailsRequest? EventsDetails { get; set; }
        public FashionDetailsRequest? FashionDetails { get; set; }
        public JobOpportunitiesDetailsRequest? JobOpportunitiesDetails { get; set; }
        public PetDetailsRequest? AnimalDetails { get; set; }
        public RealEstateDetailsRequest? RealEstateDetails { get; set; }
        public ServiceDetailsRequest? ServicesDetails { get; set; }
        public ToyDetailsRequest? ToysDetails { get; set; }
        public VehicleDetailsRequest? VehiclesDetails { get; set; }
        public OtherDetailsRequest? OthersDetails { get; set; }

    }
}
