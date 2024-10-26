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

        public CreateAdvertisementCommand(
            string accountId,
            string? type,
            string? subCategory,
            DateTime? expiresAt,
            CreateAddressCommand? address,
            CreateBeautyDetailsCommand? beautyDetails)
        {
            AccountId = accountId;
            Type = type;
            SubCategory = subCategory;
            ExpiresAt = expiresAt;
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

            RuleFor(x => x.Address)
                .NotNull()
                .WithMessage("Address must be provided.")
                .SetValidator(new CreateAddressCommandValidator()!);

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
    }
}
