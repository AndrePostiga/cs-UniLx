using UniLx.Application.Usecases.SharedModels.Responses;
using UniLx.Domain.Entities.AccountAgg;

namespace UniLx.Application.Usecases.SharedModels.Mappers
{
    public static class OwnerToOwnerSummaryResponseMapper
    {
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
