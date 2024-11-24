using FluentValidation;
using Microsoft.AspNetCore.Http;
using UniLx.Application.Usecases.Advertisements.Commands.CreateAdvertisement.Commands;
using UniLx.Application.Usecases.Advertisements.Commands.CreateAdvertisement.DetailsCommands;
using UniLx.Domain.Entities.AdvertisementAgg.Enumerations;
using UniLx.Shared.Abstractions;
using UniLx.Shared.LibExtensions;
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
        public CreateEventsDetailsCommand? EventsDetails { get; set; }
        public CreateElectronicsDetailsCommand? ElectronicsDetails { get; set; }
        public CreateFashionDetailsCommand? FashionDetails { get; set; }
        public CreateJobOpportunitiesDetailsCommand? JobOpportunitiesDetails { get; set; }
        public CreatePetDetailsCommand? PetDetails { get; set; }

        public CreateAdvertisementCommand(
            string accountId,
            string? type,
            string? subCategory,
            DateTime? expiresAt,
            CreateAddressCommand? address,
            CreateBeautyDetailsCommand? beautyDetails,
            CreateEventsDetailsCommand? eventDetails,
            CreateElectronicsDetailsCommand? electronicsDetails,
            CreateFashionDetailsCommand? fashionDetails,
            CreateJobOpportunitiesDetailsCommand? jobOpportunitiesDetails,
            CreatePetDetailsCommand? petDetails)
        {
            AccountId = accountId;
            Type = type;
            SubCategory = subCategory;
            ExpiresAt = expiresAt;
            Address = address;
            BeautyDetails = beautyDetails;
            EventsDetails = eventDetails;
            ElectronicsDetails = electronicsDetails;
            FashionDetails = fashionDetails;
            JobOpportunitiesDetails = jobOpportunitiesDetails;
            PetDetails = petDetails;
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

            AddDetailsRules<CreateBeautyDetailsCommand, CreateBeautyDetailsCommandValidator>(AdvertisementType.Beauty, nameof(CreateAdvertisementCommand.BeautyDetails));
            AddDetailsRules<CreateEventsDetailsCommand, CreateEventsDetailsCommandValidator>(AdvertisementType.Events, nameof(CreateAdvertisementCommand.EventsDetails));
            AddDetailsRules<CreateElectronicsDetailsCommand, CreateElectronicsDetailsCommandValidator>(AdvertisementType.Electronics, nameof(CreateAdvertisementCommand.ElectronicsDetails));
            AddDetailsRules<CreateFashionDetailsCommand, CreateFashionDetailsCommandValidator>(AdvertisementType.Fashion, nameof(CreateAdvertisementCommand.FashionDetails));
            AddDetailsRules<CreateJobOpportunitiesDetailsCommand, CreateJobOpportunitiesDetailsCommandValidator>(AdvertisementType.JobOpportunities, nameof(CreateAdvertisementCommand.JobOpportunitiesDetails));
            AddDetailsRules<CreatePetDetailsCommand, CreatePetDetailsCommandValidator>(AdvertisementType.Pets, nameof(CreateAdvertisementCommand.PetDetails));
        }

        private void AddDetailsRules<TDetails, TValidator>(AdvertisementType expectedType, string detailsPropertyName)
            where TDetails : class
            where TValidator : AbstractValidator<TDetails>, new()
        {
            RuleFor(x => GetDetailsProperty<TDetails>(x, detailsPropertyName))                
                .NotNull().WithMessage($"{detailsPropertyName.ToSnakeCase()} should be provided if the advertisement type is '{expectedType.Name}'.")
                .WithName("Invalid Contract")
                .When(x => AdvertisementType.TryFromName(x.Type, true, out var type) && type == expectedType)
            .SetValidator(new TValidator()!)            
                .When(x => AdvertisementType.TryFromName(x.Type, true, out var type) && type == expectedType);

            RuleFor(x => GetDetailsProperty<TDetails>(x, detailsPropertyName))
                .Null().WithMessage($"{detailsPropertyName.ToSnakeCase()} should only be provided if the advertisement type is '{expectedType.Name}'.")
                .WithName("Invalid Contract")
                .When(x => AdvertisementType.TryFromName(x.Type, true, out var type) && type != expectedType);
        }

        private static TDetails? GetDetailsProperty<TDetails>(object instance, string propertyName) where TDetails : class
        {
            var property = instance.GetType().GetProperty(propertyName);
            return property?.GetValue(instance) as TDetails;
        }
    }
}
