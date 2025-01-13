using UniLx.Domain.Entities.AdvertisementAgg.Enumerations;
using UniLx.Domain.Exceptions;

namespace UniLx.Domain.Entities.AdvertisementAgg.SpecificDetails
{
    public class PetDetails : Details
    {
        protected override AdvertisementType Type => AdvertisementType.Pets;

        public PetType PetType { get; private set; } // Sell, Adoption, Accessories
        public string AnimalType { get; private set; }  // E.g., Dog, Cat, Bird
        public int? Age { get; private set; }          // Age in years
        public string? Breed { get; private set; }     // Breed or species (raça)
        public bool? IsVaccinated { get; private set; } // Whether the pet is vaccinated
        public PetGender? Gender { get; private set; }    // Male, Female, Unknown
        public bool? IsExotic { get; private set; }     // Whether the pet is exotic

        public string? AccessoryType { get; private set; } // Type of accessory (if applicable)
        public List<string>? Materials { get; private set; } // List of materials for accessories

        public string? AdoptionRequirements { get; private set; } // Special requirements for adoption
        public string? HealthStatus { get; private set; }         // Health condition of the pet
        public bool? IsSterilized { get; private set; }           // Whether the pet is neutered/spayed



        private PetDetails() : base() { }

        public PetDetails(
            string? title,
            string? description,
            int? price,
            string petType,
            string? animalType = null,
            int? age = null,
            string? breed = null,
            bool? isVaccinated = null,
            string? gender = null,
            bool? isExotic = false,
            string? accessoryType = null,
            List<string>? materials = null,
            string? adoptionRequirements = null,
            string? healthStatus = null,
            bool? isSterilized = null
        ) : base(title, description, null)
        {
            SetPetType(petType);

            SetPrice(price);
            SetAnimalType(animalType);
            SetAge(age);
            SetBreed(breed);
            SetIsVaccinated(isVaccinated);
            SetGender(gender);
            SetIsExotic(isExotic);

            SetAccessoryType(accessoryType);
            SetMaterials(materials);

            SetAdoptionRequirements(adoptionRequirements);
            SetHealthStatus(healthStatus);
            SetIsSterilized(isSterilized);
        }

        private void SetIsExotic(bool? isExotic)
        {
            if (PetType.Name == PetType.Accessories.Name)
            {
                DomainException.ThrowIf(isExotic.HasValue, "IsExotic must not have a value for Accessories.");
                return;
            }

            IsExotic = isExotic.GetValueOrDefault();
        }

        private void SetPetType(string petType)
        {
            DomainException.ThrowIf(string.IsNullOrWhiteSpace(petType), "PetType cannot be null or empty.");
            var isValidPetType = PetType.TryFromName(petType, ignoreCase: true, out var parsedPetType);
            DomainException.ThrowIf(!isValidPetType, $"Invalid PetType. Allowed values are: {string.Join(", ", PetType.List.Select(p => p.Name))}.");
            PetType = parsedPetType!;
        }

        private void SetPrice(int? price)
        {
            if (PetType.Name == PetType.Adoption.Name)
            {
                DomainException.ThrowIf(price.HasValue && price > 0, "Adoption cannot have a price.");
                return;
            }

            Price = price;
        }

        private void SetAnimalType(string? animalType)
        {
            DomainException.ThrowIf(string.IsNullOrWhiteSpace(animalType), "AnimalType is required.");
            DomainException.ThrowIf(animalType!.Length > 100, "AnimalType must not exceed 100 characters.");
            AnimalType = animalType;
        }

        private void SetAge(int? age)
        {
            if (PetType.Name == PetType.Accessories.Name)
            {
                DomainException.ThrowIf(age.HasValue, "Age must not have a value for Accessories.");
                return;
            }

            DomainException.ThrowIf(age < 0, "Age must be a positive number.");
            Age = age;
        }

        private void SetBreed(string? breed)
        {
            if (PetType.Name == PetType.Accessories.Name)
            {
                DomainException.ThrowIf(!string.IsNullOrWhiteSpace(breed), "Breed must not have a value for Accessories.");
                return;
            }
            
            DomainException.ThrowIf(!string.IsNullOrWhiteSpace(breed) && breed.Length > 100, "Breed must not exceed 100 characters.");
            Breed = breed;
        }

        private void SetIsVaccinated(bool? isVaccinated)
        {
            if (PetType.Name == PetType.Accessories.Name)
            { 
                DomainException.ThrowIf(isVaccinated.HasValue, "IsVaccinated must not have a value for Accessories.");
                return;
            }

            IsVaccinated = isVaccinated.GetValueOrDefault();
        }

        private void SetGender(string? gender)
        {
            if (PetType.Name == PetType.Accessories.Name)
            {
                DomainException.ThrowIf(!string.IsNullOrWhiteSpace(gender), "Gender must not have a value for Accessories.");
                return;
            }

            var hasGender = PetGender.TryFromName(gender, ignoreCase: true, out var genderType);
            DomainException.ThrowIf(hasGender == false, $"Invalid pet gender type, possible values are {string.Join(",", PetGender.List)}.");
            Gender = genderType;
        }

        private void SetAccessoryType(string? accessoryType)
        {
            if (PetType.Name == PetType.Sell.Name || PetType.Name == PetType.Adoption.Name)
            {
                DomainException.ThrowIf(!string.IsNullOrWhiteSpace(accessoryType), $"AccessoryType must not have value for {PetType}.");
                return;
            }
            
            DomainException.ThrowIf(string.IsNullOrWhiteSpace(accessoryType), "AccessoryType is required for Accessories.");
            DomainException.ThrowIf(accessoryType!.Length > 100, "AccessoryType must not exceed 100 characters.");            
            AccessoryType = accessoryType;
        }

        private void SetMaterials(List<string>? materials)
        {
            if (PetType.Name == PetType.Sell.Name || PetType.Name == PetType.Adoption.Name)
            {
                DomainException.ThrowIf(materials != null && materials.Count != 0, $"Materials must only have values for {PetType}.");
                return;
            }
            
            DomainException.ThrowIf(materials == null || materials.Count == 0, "At least one material is required for Accessories.");
            DomainException.ThrowIf(materials!.Any(m => string.IsNullOrWhiteSpace(m)), "Materials cannot contain null or empty values.");
            DomainException.ThrowIf(materials!.Any(m => m.Length > 50), "Each material must not exceed 50 characters.");            
            Materials = materials;
        }

        private void SetAdoptionRequirements(string? adoptionRequirements)
        {
            if (PetType.Name == PetType.Sell.Name || PetType.Name == PetType.Accessories.Name)
            {
                DomainException.ThrowIf(!string.IsNullOrWhiteSpace(adoptionRequirements), $"AdoptionRequirements must only have a value for {PetType}.");
                return;
            }            

            DomainException.ThrowIf(string.IsNullOrWhiteSpace(adoptionRequirements), "AdoptionRequirements is required for Adoption.");
            DomainException.ThrowIf(adoptionRequirements!.Length > 512, "AdoptionRequirements must not exceed 512 characters.");            
            AdoptionRequirements = adoptionRequirements;
        }

        private void SetHealthStatus(string? healthStatus)
        {
            if (PetType.Name == PetType.Accessories.Name)
            {
                DomainException.ThrowIf(!string.IsNullOrWhiteSpace(healthStatus), "HealthStatus must not have a value for Accessories.");
                return;
            }

            DomainException.ThrowIf(string.IsNullOrWhiteSpace(healthStatus), "HealthStatus is required for Adoption.");
            DomainException.ThrowIf(healthStatus!.Length > 100, "HealthStatus must not exceed 100 characters.");            
            HealthStatus = healthStatus;
        }

        private void SetIsSterilized(bool? isSterilized)
        {
            if (PetType.Name == PetType.Accessories.Name)
            {
                DomainException.ThrowIf(isSterilized.HasValue, "IsSterilized must not have a value for Accessories.");
                return;
            }

            IsSterilized = isSterilized;
        }
    }
}
