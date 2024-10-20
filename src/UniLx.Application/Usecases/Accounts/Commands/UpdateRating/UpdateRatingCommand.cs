using FluentValidation;
using Microsoft.AspNetCore.Http;
using UniLx.Shared.Abstractions;

namespace UniLx.Application.Usecases.Accounts.Commands.UpdateRating
{
    public class UpdateRatingCommand : ICommand<IResult>
    {
        public float Rating { get; set; }
        public string AccountId { get; set; }

        public UpdateRatingCommand(float rating, string accountId)
        {
            Rating = rating;
            AccountId = accountId;
        }
    }

    public class UpdateRatingCommandValidator : AbstractValidator<UpdateRatingCommand>
    {
        public UpdateRatingCommandValidator()
        {
            RuleFor(command => command.Rating)
                .InclusiveBetween(0, 5)
                .WithMessage("Rating must be between 0 and 5.");
        }
    }
}
