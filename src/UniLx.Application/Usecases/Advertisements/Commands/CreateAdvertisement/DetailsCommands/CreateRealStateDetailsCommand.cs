using FluentValidation;
using UniLx.Domain.Entities.AdvertisementAgg.Enumerations;

namespace UniLx.Application.Usecases.Advertisements.Commands.CreateAdvertisement.DetailsCommands
{
    namespace UniLx.Application.Usecases.Advertisements.Commands.CreateAdvertisement.DetailsCommands
    {
        public class CreateRealEstateDetailsCommand
        {
            public string? Title { get; }
            public string? Description { get; }
            public int? Price { get; }

            public double LotSizeInSquareMeters { get; }
            public double? ConstructedSquareFootage { get; }
            public int? Bedrooms { get; }
            public int? Bathrooms { get; }
            public int? ParkingSpaces { get; }
            public string? PropertyType { get; }
            public string? Condition { get; }
            public int? Floors { get; }
            public List<string>? AdditionalFeatures { get; }

            public CreateRealEstateDetailsCommand(
                string? title,
                string? description,
                int? price,
                double lotSizeInSquareMeters,
                double? constructedSquareFootage,
                int? bedrooms,
                int? bathrooms,
                int? parkingSpaces,
                string? propertyType,
                string? condition,
                int? floors,
                List<string>? additionalFeatures
            )
            {
                Title = title;
                Description = description;
                Price = price;
                LotSizeInSquareMeters = lotSizeInSquareMeters;
                ConstructedSquareFootage = constructedSquareFootage;
                Bedrooms = bedrooms;
                Bathrooms = bathrooms;
                ParkingSpaces = parkingSpaces;
                PropertyType = propertyType;
                Condition = condition;
                Floors = floors;
                AdditionalFeatures = additionalFeatures;
            }
        }

        public class CreateRealEstateDetailsCommandValidator : AbstractValidator<CreateRealEstateDetailsCommand>
        {
            public CreateRealEstateDetailsCommandValidator()
            {
                RuleFor(x => x.Title)
                    .NotEmpty().WithMessage("Title is required.")
                    .MaximumLength(256).WithMessage("Title must not exceed 256 characters.");

                RuleFor(x => x.Description)
                    .MaximumLength(512).WithMessage("Description must not exceed 512 characters.")
                    .When(x => x.Description != null);

                RuleFor(x => x.Price)
                    .NotNull().WithMessage("Price is required.")
                    .GreaterThan(0).WithMessage("Price must be greater than zero.")
                    .LessThanOrEqualTo(100_000_000).WithMessage("Price must be less than or equal to 100,000,000.");

                RuleFor(x => x.LotSizeInSquareMeters)
                    .NotNull().WithMessage("LotSizeInSquareMeters is required.")
                    .GreaterThan(0).WithMessage("LotSizeInSquareMeters must be greater than zero.");

                RuleFor(x => x.ConstructedSquareFootage)
                    .GreaterThan(0).WithMessage("ConstructedSquareFootage, if provided, must be greater than zero.")
                    .When(x => x.ConstructedSquareFootage.HasValue);

                RuleFor(x => x.PropertyType)
                    .NotEmpty().WithMessage("PropertyType is required.")
                    .Must(pt => PropertyType.TryFromName(pt, true, out _))
                    .WithMessage($"Invalid property type. Supported property types are: {string.Join(", ", PropertyType.List.Select(p => p.Name))}.");
                
                When(x => x.PropertyType != null && PropertyType.TryFromName(x.PropertyType, true, out var propertyType) && propertyType.Name == PropertyType.Land.Name, () =>
                {
                    RuleFor(x => x.Condition)
                        .Must(cond => string.IsNullOrWhiteSpace(cond))
                        .WithMessage("Condition must not have a value for land type.");

                    RuleFor(x => x.Bedrooms)
                        .Null().WithMessage("Bedrooms must not have a value for land type.");

                    RuleFor(x => x.Bathrooms)
                        .Null().WithMessage("Bathrooms must not have a value for land type.");

                    RuleFor(x => x.ParkingSpaces)
                        .Null().WithMessage("ParkingSpaces must not have a value for land type.");

                    RuleFor(x => x.Floors)
                        .Null().WithMessage("Floors must not have a value for land type.");
                });

                When(x => x.PropertyType != null && PropertyType.TryFromName(x.PropertyType, true, out var nonLand) && nonLand.Name != PropertyType.Land.Name, () =>
                {
                    RuleFor(x => x.Condition)
                        .NotEmpty().WithMessage("Condition is required for non-land properties.")
                        .Must(cond => PropertyCondition.TryFromName(cond, true, out _))
                        .WithMessage($"Invalid condition. Supported conditions are: {string.Join(", ", PropertyCondition.List.Select(c => c.Name))}.");

                    RuleFor(x => x.Bedrooms)
                        .GreaterThanOrEqualTo(0).WithMessage("Bedrooms cannot be negative.")
                        .LessThanOrEqualTo(100).WithMessage("Bedrooms number seems too high.")
                        .When(x => x.Bedrooms.HasValue);

                    RuleFor(x => x.Bathrooms)
                        .GreaterThanOrEqualTo(0).WithMessage("Bathrooms cannot be negative.")
                        .LessThanOrEqualTo(50).WithMessage("Bathrooms number seems too high.")
                        .When(x => x.Bathrooms.HasValue);

                    RuleFor(x => x.ParkingSpaces)
                        .GreaterThanOrEqualTo(0).WithMessage("Parking spaces cannot be negative.")
                        .LessThanOrEqualTo(100).WithMessage("Parking spaces number seems too high.")
                        .When(x => x.ParkingSpaces.HasValue);

                    RuleFor(x => x.Floors)
                        .GreaterThanOrEqualTo(0).WithMessage("Floors cannot be negative.")
                        .LessThanOrEqualTo(100).WithMessage("Floors number seems too high.")
                        .When(x => x.Floors.HasValue);
                });
                
                RuleForEach(x => x.AdditionalFeatures)
                    .NotEmpty().WithMessage("Additional feature cannot be empty.")
                    .MaximumLength(50).WithMessage("Each additional feature must be 50 characters or less.")
                    .When(x => x.AdditionalFeatures != null && x.AdditionalFeatures.Count != 0);
            }
        }
    }

}
