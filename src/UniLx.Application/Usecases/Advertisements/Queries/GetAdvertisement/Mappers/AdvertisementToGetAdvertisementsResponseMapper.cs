using UniLx.Application.Usecases.Advertisements.Queries.GetAdvertisement.Models;
using UniLx.Application.Usecases.Advertisements.SharedModels.Responses;
using UniLx.Application.Usecases.Advertisements.SharedModels.Responses.DetailsResponse;
using UniLx.Domain.Entities.AdvertisementAgg;
using UniLx.Domain.Entities.AdvertisementAgg.SpecificDetails;
using UniLx.Domain.Entities.Seedwork;

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
                BeautyDetails = source.Details.TryMapDetails(out var details) ? (BeautyDetailsResponse?)details : null,
            };

        public static AddressResponse ToResponse(this Address source)
            => new()
            {
                City = source.City,
                Country = source.Country,
                Latitude = source.Latitude,
                Longitude = source.Longitude,
                Neighborhood = source.Neighborhood,
                State = source.State,
                ZipCode = source.ZipCode
            };

        public static bool TryMapDetails(this Details details, out BaseDetailsResponse? detailsResponse)
        {
            detailsResponse = null;
            if (details == null)
                return false;

            if (details is BeautyDetails beautyDetails)
                detailsResponse = beautyDetails.ToResponse();

            return true;
        }

        public static BeautyDetailsResponse ToResponse(this BeautyDetails source)
            => new()
            {
                Title = source.Title,
                Price = source.Price,
                Description = source.Description,

                Brand = source.Brand,
                SkinType = source.SkinType,
                IsOrganic = source.IsOrganic,
                ProductType = source.ProductType,
                Ingredients = source.Ingredients,
                ExpirationDate = source.ExpirationDate,
            };
    }
}
