using UniLx.Application.Usecases.Accounts.Queries.GetAccountAdvertisements.Models;
using UniLx.Application.Usecases.SharedModels.Mappers;
using UniLx.Application.Usecases.SharedModels.Responses.DetailsResponse;
using UniLx.Domain.Entities.AdvertisementAgg;
using UniLx.Domain.Entities.AdvertisementAgg.Enumerations;
using UniLx.Domain.Entities.AdvertisementAgg.SpecificDetails;

namespace UniLx.Application.Usecases.Accounts.Queries.GetAccountAdvertisements.Mappers
{
    internal static class AdvertisementToGetAccountAdvertisementsResponseMapper
    {
        public static GetAccountAdvertisementsResponse ToResponse(this Advertisement source)
            => new()
            {
                Id = source.Id,
                Status = source.Status.Name,
                Type = source.Type.Name,
                Category = source.CategoryName,
                ExpiresAt = source.ExpiresAt,
                CreatedAt = source.CreatedAt,
                UpdatedAt = source.UpdatedAt,
                Rating = source.Rating.ToResponse(),
                Title = source.Details.Title,
                Description = source.Details.Description,
                Price = source.Details.Price,
                Images = source.Details.Images,
                Address = source.Address.ToResponse(),
                BeautyDetails = source.Type == AdvertisementType.Beauty ? (BeautyDetailsResponse?)(source.Details as BeautyDetails)!.ToResponse() : null,
                EventsDetails = source.Type == AdvertisementType.Events ? (EventsDetailsResponse?)(source.Details as EventsDetails)!.ToResponse() : null,
                ElectronicsDetails = source.Type == AdvertisementType.Electronics ? (ElectronicsDetailsResponse?)(source.Details as ElectronicsDetails)!.ToResponse() : null,
                FashionDetails = source.Type == AdvertisementType.Fashion ? (FashionDetailsResponse?)(source.Details as FashionDetails)!.ToResponse() : null,
                JobOpportunitiesDetails = source.Type == AdvertisementType.JobOpportunities ? (JobOpportunitiesDetailsResponse?)(source.Details as JobOpportunitiesDetails)!.ToResponse() : null,
                PetDetails = source.Type == AdvertisementType.Pets ? (PetDetailsResponse?)(source.Details as PetDetails)!.ToResponse() : null,
                RealEstateDetails = source.Type == AdvertisementType.RealEstate ? (RealEstateDetailsResponse?)(source.Details as RealEstateDetails)!.ToResponse() : null,
                OthersDetails = source.Type == AdvertisementType.Others ? (OthersDetailsResponse?)(source.Details as OthersDetails)!.ToResponse() : null,
            };
    }
}
