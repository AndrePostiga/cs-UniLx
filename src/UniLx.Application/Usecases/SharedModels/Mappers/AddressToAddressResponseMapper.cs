using UniLx.Application.Usecases.SharedModels.Responses;
using UniLx.Domain.Entities.Seedwork;

namespace UniLx.Application.Usecases.SharedModels.Mappers
{
    public static class AddressToAddressResponseMapper
    {
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
    }
}
