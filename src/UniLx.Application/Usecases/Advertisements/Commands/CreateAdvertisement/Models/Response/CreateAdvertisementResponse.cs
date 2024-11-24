using UniLx.Application.Usecases.SharedModels.Responses;
using UniLx.Application.Usecases.SharedModels.Responses.DetailsResponse;

namespace UniLx.Application.Usecases.Advertisements.Commands.CreateAdvertisement.Models.Response
{
    internal class CreateAdvertisementResponse
    {
        public string Id { get; set; }
        public OwnerSummaryResponse Owner { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public string Category { get; set; }
        public DateTime ExpiresAt { get; set; }
        public AddressResponse Address { get; set; }        
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public BeautyDetailsResponse? BeautyDetails { get; set; }
        public EventsDetailsResponse? EventsDetails { get; set; }
        public ElectronicsDetailsResponse? ElectronicsDetails { get; set; }
        public FashionDetailsResponse? FashionDetails { get; set; }
    }
}
