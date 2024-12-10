using UniLx.Application.Usecases.Accounts.Commands.UpdateRating.Models;
using UniLx.Domain.Entities.AccountAgg;

namespace UniLx.Application.Usecases.Accounts.Commands.UpdateRating.Mappers
{
    internal static class RatingToUpdateRatingRequest
    {
        internal static UpdateRatingResponse ToResponse(this Rating source) => 
            new(rating: source.Value, source.Count);
    }
}
