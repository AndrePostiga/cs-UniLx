using UniLx.Application.Usecases.SharedModels.Responses;
using UniLx.Domain.Entities.Seedwork.ValueObj;

namespace UniLx.Application.Usecases.SharedModels.Mappers
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
