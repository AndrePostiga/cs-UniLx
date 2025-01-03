using UniLx.Application.Usecases.SharedModels.Responses;
using UniLx.Domain.Entities.Seedwork;

namespace UniLx.Application.Usecases.SharedModels.Mappers
{
    public static class RatingToRatingResponseMapper
    {
        public static RatingResponse ToResponse(this Rating source)
            => new()
            {
                Rating = source.Value,
                Votes = source.Count
            };
    }
}
