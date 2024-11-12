using UniLx.Application.Usecases.Advertisements.SharedModels.Responses;
using UniLx.Domain.Entities.Seedwork.ValueObj;

namespace UniLx.Application.Usecases.Advertisements.SharedModels.Mappers
{
    public static class ContactInformationToContactInformationResponseMapper
    {
        public static ContactInformationResponse ToResponse(this ContactInformation source)
            => new()
            {
                Email = source.Email?.Value,
                Phone = source.Phone?.ToString(),
                Website = source.Website?.ToString(),
            };
    }
}
