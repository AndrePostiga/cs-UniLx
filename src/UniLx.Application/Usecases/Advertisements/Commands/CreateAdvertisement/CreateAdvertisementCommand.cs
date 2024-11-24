using FluentValidation;
using Microsoft.AspNetCore.Http;
using UniLx.Application.Usecases.Advertisements.Commands.CreateAdvertisement.Commands;
using UniLx.Application.Usecases.Advertisements.Commands.CreateAdvertisement.DetailsCommand;
using UniLx.Domain.Entities.AdvertisementAgg.Enumerations;
using UniLx.Shared.Abstractions;
using static UniLx.Application.Usecases.Advertisements.Commands.CreateAdvertisement.Commands.CreateAddressCommand;

namespace UniLx.Application.Usecases.Advertisements.Commands.CreateAdvertisement
{
    public class CreateAdvertisementCommand : ICommand<IResult>
    {
        public string? AccountId { get; set; }
        public string? Type { get; set; }
        public string? SubCategory { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public CreateAddressCommand? Address { get; set; }

        public CreateBeautyDetailsCommand? BeautyDetails { get; set; }
        public CreateEventDetailsCommand? EventsDetails { get; set; }

        public CreateAdvertisementCommand(
            string accountId,
            string? type,
            string? subCategory,
            DateTime? expiresAt,
            CreateAddressCommand? address,
            CreateBeautyDetailsCommand? beautyDetails,
            CreateEventDetailsCommand? eventDetails)
        {
            AccountId = accountId;
            Type = type;
            SubCategory = subCategory;
            ExpiresAt = expiresAt;
            Address = address;
            BeautyDetails = beautyDetails;
            EventsDetails = eventDetails;
        }
    }

    public class CreateAdvertisementCommandValidator : AbstractValidator<CreateAdvertisementCommand>
    {
        public CreateAdvertisementCommandValidator()
        {
            RuleFor(x => x.Type)
                .NotEmpty().WithMessage("Advertisement type is required.")
                .Must(type => AdvertisementType.TryFromName(type, true, out _))
                .WithMessage($"Invalid advertisement type. Supported types are: {string.Join(", ", AdvertisementType.List)}");

            RuleFor(x => x.SubCategory)
                .NotEmpty().WithMessage("SubCategory name is required.")
                .MaximumLength(100).WithMessage("SubCategory must not exceed 100 characters.");

            RuleFor(x => x.ExpiresAt)
                .GreaterThan(DateTime.UtcNow).WithMessage("Expiration date must be in the future.")
                .When(x => x.ExpiresAt.HasValue);

            RuleFor(x => x.Address)
                .NotNull()
                .WithMessage("Address must be provided.")
                .SetValidator(new CreateAddressCommandValidator()!);

            AddBeautyRules();
            AddEventsRules();

        }

        private void AddEventsRules()
        {
            RuleFor(x => x.EventsDetails)
                .NotNull().WithMessage($"events_details should be provided if the advertisement type is '{AdvertisementType.Events.Name}'.")
                    .When(x => AdvertisementType.TryFromName(x.Type, true, out var @type) && @type == AdvertisementType.Events)
                .SetValidator(new CreateEventDetailsCommandValidator()!)
                    .When(x => AdvertisementType.TryFromName(x.Type, true, out var @type) && @type == AdvertisementType.Events);

            RuleFor(x => x.EventsDetails)
                .Null().WithMessage($"events_details should only be provided if the advertisement type is '{AdvertisementType.Events.Name}'.")
                .When(x => AdvertisementType.TryFromName(x.Type, true, out var @type) && @type != AdvertisementType.Events);
        }

        private void AddBeautyRules()
        {
            RuleFor(x => x.BeautyDetails)
                .NotNull().WithMessage($"beauty_details should be provided if the advertisement type is '{AdvertisementType.Beauty.Name}'.")
                    .When(x => AdvertisementType.TryFromName(x.Type, true, out var @type) && @type == AdvertisementType.Beauty)
                .SetValidator(new CreateBeautyDetailsCommandValidator()!)
                    .When(x => AdvertisementType.TryFromName(x.Type, true, out var @type) && @type == AdvertisementType.Beauty);

            RuleFor(x => x.BeautyDetails)
                .Null().WithMessage($"beauty_details should only be provided if the advertisement type is '{AdvertisementType.Beauty.Name}'.")
                .When(x => AdvertisementType.TryFromName(x.Type, true, out var @type) && @type != AdvertisementType.Beauty);
        }
    }
}
