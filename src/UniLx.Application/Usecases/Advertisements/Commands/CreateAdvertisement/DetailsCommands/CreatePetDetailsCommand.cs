using FluentValidation;
using UniLx.Domain.Entities.AdvertisementAgg.Enumerations;

namespace UniLx.Application.Usecases.Advertisements.Commands.CreateAdvertisement.DetailsCommands
{
    public class CreatePetDetailsCommand
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int? Price { get; set; }
        public string? PetType { get; set; }

        public string? AnimalType { get; set; }
        public int? Age { get; set; }
        public string? Breed { get; set; }
        public bool? IsVaccinated { get; set; }
        public string? Gender { get; set; }
        public bool? IsExotic { get; set; }
        public string? HealthStatus { get; set; }
        public bool? IsSterilized { get; set; }

        public string? AdoptionRequirements { get; set; }


        public string? AccessoryType { get; set; }
        public List<string>? Materials { get; set; }

        public CreatePetDetailsCommand(string? title, string? description, int? price, string? petType, string? animalType, 
            int? age, string? breed, bool? isVaccinated, string? gender, bool? isExotic, string? accessoryType, 
            List<string>? materials, string? adoptionRequirements, string? healthStatus, bool? isSterilized)
        {
            Title = title;
            Description = description;
            Price = price;
            PetType = petType;
            AnimalType = animalType;
            Age = age;
            Breed = breed;
            IsVaccinated = isVaccinated;
            Gender = gender;
            IsExotic = isExotic;
            AccessoryType = accessoryType;
            Materials = materials;
            AdoptionRequirements = adoptionRequirements;
            HealthStatus = healthStatus;
            IsSterilized = isSterilized;
        }
    }

    public class CreatePetDetailsCommandValidator : AbstractValidator<CreatePetDetailsCommand>
    {
        public CreatePetDetailsCommandValidator()
        {
            RuleFor(x => x.Title)
               .NotEmpty().WithMessage("Title is required.")
               .MaximumLength(256).WithMessage("Title must not exceed 256 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(512).WithMessage("Description must not exceed 512 characters.");

            RuleFor(x => x.PetType)
                .NotEmpty().WithMessage("PetType is required.")
                .Must(type => PetType.TryFromName(type, true, out _))
                .WithMessage($"Invalid PetType. Allowed values are: {string.Join(", ", PetType.List.Select(p => p.Name))}.");

            RuleFor(x => x.AnimalType)
                .NotEmpty().WithMessage("AnimalType is required for all scenarios.")
                .MaximumLength(100).WithMessage("AnimalType must not exceed 100 characters.");

            
            When(x => x.PetType == "Adoption", () =>
            {
                RuleFor(x => x.Price)
                    .Must(price => price == null || price == 0)
                    .WithMessage("Adoption cannot have a price.");

                RuleFor(x => x.AdoptionRequirements)
                    .NotEmpty().WithMessage("AdoptionRequirements is required.")
                    .MaximumLength(512).WithMessage("AdoptionRequirements must not exceed 512 characters.");

                // Ensure accessory-related fields are empty
                RuleFor(x => x.AccessoryType)
                    .Empty().WithMessage("AccessoryType must not have a value for Adoption.");

                RuleFor(x => x.Materials)
                    .Null().WithMessage("Materials must not have values for Adoption.");
            });

            When(x => x.PetType == "Accessories", () =>
            {
                RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Price must be greater than or equal to 0.")
                .LessThanOrEqualTo(100_000_000).WithMessage("Price must be less than or equal to 100,000,000.")
                .When(x => x.Price.HasValue);

                RuleFor(x => x.AccessoryType)
                    .NotEmpty().WithMessage("AccessoryType is required for Accessories.")
                    .MaximumLength(100).WithMessage("AccessoryType must not exceed 100 characters.");

                RuleFor(x => x.Materials)
                    .NotEmpty().WithMessage("Materials are required for Accessories.")
                    .Must(materials => materials!.All(m => !string.IsNullOrWhiteSpace(m)))
                    .WithMessage("Materials cannot contain null or empty values.")
                    .Must(materials => materials!.All(m => m.Length <= 50))
                    .WithMessage("Each material must not exceed 50 characters.");

                
                RuleFor(x => x.Age)
                    .Null().WithMessage("Age must not have a value for Accessories.");

                RuleFor(x => x.Breed)
                    .Empty().WithMessage("Breed must not have a value for Accessories.");

                RuleFor(x => x.IsVaccinated)
                    .Null().WithMessage("IsVaccinated must not have a value for Accessories.");

                RuleFor(x => x.Gender)
                    .Empty().WithMessage("Gender must not have a value for Accessories.");

                RuleFor(x => x.IsExotic)
                    .Equal(false).WithMessage("IsExotic must not be true for Accessories.");

                RuleFor(x => x.HealthStatus)
                    .Empty().WithMessage("HealthStatus must not have a value for Accessories.");

                RuleFor(x => x.IsSterilized)
                    .Null().WithMessage("IsSterilized must not have a value for Accessories.");

                RuleFor(x => x.AdoptionRequirements)
                    .Empty().WithMessage("AdoptionRequirements must not have a value for Accessories.");
            });

            When(x => x.PetType == "Sell", () =>
            {

                RuleFor(x => x.Price)
                    .NotEmpty()
                    .GreaterThanOrEqualTo(0).WithMessage("Price must be greater than or equal to 0.")
                    .LessThanOrEqualTo(100_000_000).WithMessage("Price must be less than or equal to 100,000,000.");

                RuleFor(x => x.AdoptionRequirements)
                    .Empty().WithMessage("AdoptionRequirements must not have a value for Sell.");

                RuleFor(x => x.AccessoryType)
                    .Empty().WithMessage("AccessoryType must not have a value for Sell.");

                RuleFor(x => x.Materials)
                    .Null().WithMessage("Materials must not have values for Sell.");
            });
        }
    }
}
