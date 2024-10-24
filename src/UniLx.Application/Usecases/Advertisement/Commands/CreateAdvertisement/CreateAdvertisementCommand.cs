using FluentValidation;
using Microsoft.AspNetCore.Http;
using UniLx.Application.Usecases.Advertisement.Commands.CreateAdvertisement.DetailsCommand;
using UniLx.Application.Usecases.Advertisement.Commands.CreateAdvertisement.Models;
using UniLx.Domain.Entities.AdvertisementAgg.Enumerations;
using UniLx.Shared.Abstractions;

namespace UniLx.Application.Usecases.Advertisement.Commands.CreateAdvertisement
{
    public class CreateAdvertisementCommand : ICommand<IResult>
    {
        public string? AccountId { get; set; }
        public string? Type { get; set; }
        public string? SubCategory { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public AddressRequest? Address { get; set; }

        public CreateBeautyDetailsCommand? BeautyDetails { get; set; }

        public CreateAdvertisementCommand(
            string accountId,
            string? type,
            string? subCategory,
            DateTime? expiresAt,
            double? latitude,
            double? longitude,
            AddressRequest? address,
            CreateBeautyDetailsCommand? beautyDetails)
        {
            AccountId = accountId;
            Type = type;
            SubCategory = subCategory;
            ExpiresAt = expiresAt;
            Latitude = latitude;
            Longitude = longitude;
            Address = address;
            BeautyDetails = beautyDetails;
        }
    }

    public class CreateAdvertisementCommandValidator : AbstractValidator<CreateAdvertisementCommand>
    {
        public CreateAdvertisementCommandValidator()
        {
            // Type validation
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

            RuleFor(x => x)
                .Must(HaveBothLatitudeAndLongitude).WithMessage("Both Latitude and Longitude must be provided.");

            RuleFor(x => x.Latitude)
                .InclusiveBetween(-90, 90).WithMessage("Latitude must be between -90 and 90.")
                .When(x => x.Latitude.HasValue);

            RuleFor(x => x.Longitude)
                .InclusiveBetween(-180, 180).WithMessage("Longitude must be between -180 and 180.")
                .When(x => x.Longitude.HasValue);

            RuleFor(x => x.BeautyDetails)
            .NotNull()
            .When(x => AdvertisementType.TryFromName(x.Type, true, out var @type) && @type == AdvertisementType.Beauty)
            .WithMessage($"Beauty details should be provided if the advertisement type is '{AdvertisementType.Beauty.Name}'.")
            .SetValidator(new CreateBeautyDetailsCommandValidator()!)
            .When(x => AdvertisementType.TryFromName(x.Type, true, out var @type) && @type == AdvertisementType.Beauty)            
            .Null()
            .When(x => AdvertisementType.TryFromName(x.Type, true, out var @type) && @type != AdvertisementType.Beauty)
            .WithMessage($"Beauty details should only be provided if the advertisement type is '{AdvertisementType.Beauty.Name}'.");
        }

        private bool HaveBothLatitudeAndLongitude(CreateAdvertisementCommand command)
            => (command.Latitude.HasValue && command.Longitude.HasValue) || (!command.Latitude.HasValue && !command.Longitude.HasValue);
    }
}
