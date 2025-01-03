using UniLx.Application.Usecases.SharedModels.Responses;
using UniLx.Application.Usecases.SharedModels.Responses.DetailsResponse;

namespace UniLx.Application.Usecases.Accounts.Queries.GetAccountAdvertisements.Models
{
    internal class GetAccountAdvertisementsResponse
    {
        public string Id { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public string Category { get; set; }
        public RatingResponse Rating { get; set; }
        public DateTime ExpiresAt { get; set; }
        public AddressResponse Address { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public int? Price { get; set; }
        public List<string>? Images { get; set; }
        public BeautyDetailsResponse? BeautyDetails { get; set; }
        public EventsDetailsResponse? EventsDetails { get; set; }
        public ElectronicsDetailsResponse? ElectronicsDetails { get; set; }
        public FashionDetailsResponse? FashionDetails { get; set; }
        public JobOpportunitiesDetailsResponse? JobOpportunitiesDetails { get; set; }
        public PetDetailsResponse? PetDetails { get; set; }
        public RealEstateDetailsResponse? RealEstateDetails { get; set; }
        public OthersDetailsResponse? OthersDetails { get; set; }
    }
}
