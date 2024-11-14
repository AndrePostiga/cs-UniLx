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
                Address = source.Address.ToResponse(),
                BeautyDetails = source.Type == AdvertisementType.Beauty ? (BeautyDetailsResponse?)(source.Details as BeautyDetails)!.ToResponse() : null,
                EventsDetails = source.Type == AdvertisementType.Events ? (EventsDetailsResponse?)(source.Details as EventsDetails)!.ToResponse() : null,
            };
    }
}
