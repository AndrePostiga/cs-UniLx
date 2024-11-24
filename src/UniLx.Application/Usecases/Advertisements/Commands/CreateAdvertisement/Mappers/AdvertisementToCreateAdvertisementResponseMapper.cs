using UniLx.Application.Usecases.Advertisements.Commands.CreateAdvertisement.Models.Response;
using UniLx.Application.Usecases.Advertisements.Queries.GetAdvertisement.Mappers;
using UniLx.Application.Usecases.SharedModels.Mappers;
using UniLx.Application.Usecases.SharedModels.Responses;
using UniLx.Application.Usecases.SharedModels.Responses.DetailsResponse;
using UniLx.Domain.Entities.AccountAgg;
using UniLx.Domain.Entities.AdvertisementAgg;
using UniLx.Domain.Entities.AdvertisementAgg.Enumerations;
using UniLx.Domain.Entities.AdvertisementAgg.SpecificDetails;

namespace UniLx.Application.Usecases.Advertisements.Commands.CreateAdvertisement.Mappers
{
    internal static class AdvertisementToCreateAdvertisementResponseMapper
    {
        public static CreateAdvertisementResponse ToResponse(this Advertisement source, Account owner, string? ownerProfilePicture)
            => new()
            {
                Id = source.Id,
                Owner = owner.ToResponse(ownerProfilePicture),
                Status = source.Status.Name,
                Type = source.Type.Name,
                Category = source.CategoryName,
                ExpiresAt = source.ExpiresAt,
                CreatedAt = source.CreatedAt,
                UpdatedAt = source.UpdatedAt,
                Address = source.Address.ToResponse(),
                BeautyDetails = source.Type == AdvertisementType.Beauty ? (BeautyDetailsResponse?)(source.Details as BeautyDetails)!.ToResponse() : null,
                EventsDetails = source.Type == AdvertisementType.Events ? (EventsDetailsResponse?)(source.Details as EventsDetails)!.ToResponse() : null,
                ElectronicsDetails = source.Type == AdvertisementType.Electronics ? (ElectronicsDetailsResponse?)(source.Details as ElectronicsDetails)!.ToResponse() : null,
                FashionDetails = source.Type == AdvertisementType.Fashion ? (FashionDetailsResponse?)(source.Details as FashionDetails)!.ToResponse() : null,
                JobOpportunitiesDetails = source.Type == AdvertisementType.JobOpportunities ? (JobOpportunitiesDetailsResponse?)(source.Details as JobOpportunitiesDetails)!.ToResponse() : null,
            };

        public static OwnerSummaryResponse ToResponse(this Account source, string? ownerProfilePicture)
            => new()
            {
                Id = source.Id,
                Description = source.Description,
                Email = source.Email.Value,
                Name = source.Name,
                ProfilePictureUrl = ownerProfilePicture
            };        
    }
}
