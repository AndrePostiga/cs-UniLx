using FluentValidation;
using Microsoft.AspNetCore.Http;
using UniLx.Shared.Abstractions;

namespace UniLx.Application.Usecases.Advertisements.Commands.RateAdvertisement
{
    public class RateAdvertisementCommand : ICommand<IResult>
    {
        public float Rating { get; set; }
        public string AccountId { get; set; }
        public string AdvertisementId { get; set; }

        public RateAdvertisementCommand(float rating, string accountId, string advertisementId)
        {
            Rating = rating;
            AccountId = accountId;
            AdvertisementId = advertisementId;
        }
    }

    public class RateAdvertisementCommandValidator : AbstractValidator<RateAdvertisementCommand>
    {
        public RateAdvertisementCommandValidator()
        {
            RuleFor(command => command.Rating)
                .InclusiveBetween(0, 5)
                .WithMessage("Rating must be between 0 and 5.");

            RuleFor(command => command.AccountId)
                .NotEmpty()
                .WithMessage("AccountId is required.");

            RuleFor(command => command.AdvertisementId)
                .NotEmpty()
                .WithMessage("AdvertisementId is required.");
        }
    }
}
