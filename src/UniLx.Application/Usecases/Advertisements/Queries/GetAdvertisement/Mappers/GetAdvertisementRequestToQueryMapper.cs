using UniLx.Application.Usecases.Advertisements.Queries.GetAdvertisement.Models;

namespace UniLx.Application.Usecases.Advertisements.Queries.GetAdvertisement.Mappers
{
    public static class GetAdvertisementRequestToQueryMapper
    {
        public static GetAdvertisementsQuery ToQuery(this GetAdvertisementsRequest source)
            => new(type: source.Type, 
                categoryName: source.CategoryName,
                latitude: source.Latitude,
                longitude: source.Longitude,
                radiusInKm: source.RadiusInKm,
                page: source.Page,
                pageSize: source.PageSize);
    }
}
