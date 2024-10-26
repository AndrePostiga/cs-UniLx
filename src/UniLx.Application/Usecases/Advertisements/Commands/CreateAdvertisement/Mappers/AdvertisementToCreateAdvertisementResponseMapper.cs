using UniLx.Application.Usecases.Advertisements.Commands.CreateAdvertisement.Models.Response;
using UniLx.Application.Usecases.Advertisements.SharedModels.Responses;
using UniLx.Application.Usecases.Advertisements.SharedModels.Responses.DetailsResponse;
using UniLx.Domain.Entities.AccountAgg;
using UniLx.Domain.Entities.AdvertisementAgg;
using UniLx.Domain.Entities.AdvertisementAgg.SpecificDetails;
using UniLx.Domain.Entities.Seedwork;

namespace UniLx.Application.Usecases.Advertisements.Commands.CreateAdvertisement.Mappers
{
    internal static class AdvertisementToGetAdvertisementsResponseMapper
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
                BeautyDetails = source.Details.TryMapDetails(out var details) ? (BeautyDetailsResponse?)details : null,
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

        public static AddressResponse ToResponse(this Address source)
            => new()
            {
                City = source.City,
                Complement = source.Complement,
                Country = source.Country,
                Latitude = source.Latitude,
                Longitude = source.Longitude,
                Neighborhood = source.Neighborhood,
                Number = source.Number,
                State = source.State,
                Street = source.Street,
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
