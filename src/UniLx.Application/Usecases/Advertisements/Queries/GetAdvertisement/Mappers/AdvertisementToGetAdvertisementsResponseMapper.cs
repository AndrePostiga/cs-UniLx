﻿using UniLx.Application.Usecases.Advertisements.Queries.GetAdvertisement.Models;
using UniLx.Application.Usecases.SharedModels.Mappers;
using UniLx.Application.Usecases.SharedModels.Responses;
using UniLx.Application.Usecases.SharedModels.Responses.DetailsResponse;
using UniLx.Domain.Entities.AdvertisementAgg;
using UniLx.Domain.Entities.AdvertisementAgg.Enumerations;
using UniLx.Domain.Entities.AdvertisementAgg.SpecificDetails;

namespace UniLx.Application.Usecases.Advertisements.Queries.GetAdvertisement.Mappers
{
    internal static class AdvertisementToGetAdvertisementsResponseMapper
    {
        public static GetAdvertisementsResponse ToResponse(this Advertisement source)
            => new()
            {
                Id = source.Id,
                Owner = new OwnerSummaryResponse { Id = source.OwnerId },
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
                PetDetails = source.Type == AdvertisementType.Pets ? (PetDetailsResponse?)(source.Details as PetDetails)!.ToResponse() : null,
            };
    }
}
