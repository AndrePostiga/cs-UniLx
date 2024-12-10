using UniLx.Application.Usecases.SharedModels.Responses.DetailsResponse;
using UniLx.Domain.Entities.AdvertisementAgg.SpecificDetails;

namespace UniLx.Application.Usecases.Advertisements.SharedModels.Mappers
{
    public static class DetailsToSpecificDetailsResponseMapper
    {
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

        public static EventsDetailsResponse ToResponse(this EventsDetails source)
            => new()
            {
                Title = source.Title,
                Price = source.Price,
                Description = source.Description,

                EventType = source.EventType,
                EventDate = source.EventDate,
                Organizer = source.Organizer,
                DressCode = source.DressCode,
                Highlights = source.Highlights,
                IsOnline = source.IsOnline,
                ContactInfo = source.ContactInfo.ToResponse(),
                AgeRestriction = source.AgeRestriction.Name,

            };
    }
}
