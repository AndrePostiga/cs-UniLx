using FluentValidation;
using Microsoft.AspNetCore.Http;
using UniLx.Application.Usecases.Advertisements.Commands.RateAdvertisement;
using UniLx.Shared.Abstractions;

namespace UniLx.Application.Usecases.Advertisements.Commands.FinishAdvertisement
{
    public class FinishAdvertisementCommand : ICommand<IResult>
    {
        public string OwnerId { get; set; }
        public string AdvertisementId { get; set; }

        public FinishAdvertisementCommand(string ownerId, string advertisementId)
        {
            OwnerId = ownerId;
            AdvertisementId = advertisementId;
        }
    }

    public class FinishAdvertisementCommandValidator : AbstractValidator<FinishAdvertisementCommand>
    {
        public FinishAdvertisementCommandValidator()
        {

            RuleFor(command => command.OwnerId)
                .NotEmpty()
                .WithMessage("OwnerId is required.");

            RuleFor(command => command.AdvertisementId)
                .NotEmpty()
                .WithMessage("AdvertisementId is required.");
        }
    }
}
