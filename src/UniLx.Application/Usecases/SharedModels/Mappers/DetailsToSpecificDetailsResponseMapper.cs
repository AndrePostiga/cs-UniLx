using UniLx.Application.Usecases.SharedModels.Responses.DetailsResponse;
using UniLx.Domain.Entities.AdvertisementAgg.SpecificDetails;

namespace UniLx.Application.Usecases.SharedModels.Mappers
{
    public static class DetailsToSpecificDetailsResponseMapper
    {
        public static BeautyDetailsResponse ToResponse(this BeautyDetails source)
            => new()
            {
                Title = source.Title,
                Price = source.Price,
                Description = source.Description,

                Brand = source.Brand,
                SkinType = source.SkinType,
                IsOrganic = source.IsOrganic,
                ProductType = source.ProductType,
                Ingredients = source.Ingredients,
                ExpirationDate = source.ExpirationDate,
            };

        public static EventsDetailsResponse ToResponse(this EventsDetails source)
            => new()
            {
                Title = source.Title,
                Price = source.Price,
                Description = source.Description,

                EventType = source.EventType,
                EventDate = source.EventDate,
                Organizer = source.Organizer,
                DressCode = source.DressCode,
                Highlights = source.Highlights,
                IsOnline = source.IsOnline,
                ContactInfo = source.ContactInfo.ToResponse(),
                AgeRestriction = source.AgeRestriction.Name,

            };
      
        public static ElectronicsDetailsResponse ToResponse(this ElectronicsDetails source)
            => new()
            {
                Title = source.Title,
                Price = source.Price,
                Description = source.Description,

                ProductType = source.ProductType,
                Brand = source.Brand,
                Model = source.Model,
                StorageCapacity = source.StorageCapacity,
                Memory = source.Memory,
                Processor = source.Processor,
                GraphicsCard = source.GraphicsCard,
                BatteryLife = source.BatteryLife,
                HasWarranty = source.HasWarranty,
                WarrantyUntil = source.WarrantyUntil,
                Features = source.Features,
                Condition = source.Condition.Name,
                IncludesOriginalBox = source.IncludesOriginalBox,
                Accessories = source.Accessories,
            };

        public static FashionDetailsResponse ToResponse(this FashionDetails source)
            => new()
            {
                Title = source.Title,
                Price = source.Price,
                Description = source.Description,

                ClothingType = source.ClothingType,
                Brand = source.Brand,
                Sizes = source.Sizes.Select(s => s.Name).ToList(),
                Gender = source.Gender.Name,
                Colors = source.Colors,
                Materials = source.Materials,
                Features = source.Features,
                Designer = source.Designer,
                IsHandmade = source.IsHandmade,
                ReleaseDate = source.ReleaseDate,
                IsSustainable = source.IsSustainable,
            };

        public static JobOpportunitiesDetailsResponse ToResponse(this JobOpportunitiesDetails source)
            => new()
            {
                Title = source.Title,
                Description = source.Description,

                Position = source.Position,
                Company = source.Company,
                Salary = source.Salary,
                IsSalaryDisclosed = source.IsSalaryDisclosed,
                WorkLocation = source.WorkLocation.Name,
                EmploymentType = source.EmploymentType.Name,
                ExperienceLevel = source.ExperienceLevel,
                Skills = source.Skills,
                Benefits = source.Benefits,
                RelocationHelp = source.RelocationHelp,
                ApplicationDeadline = source.ApplicationDeadline,
                ContactInfo = source.ContactInfo.ToResponse(),
            };

        public static PetDetailsResponse ToResponse(this PetDetails source)
            => new()
            {
                Title = source.Title,
                Price = source.Price,
                Description = source.Description,

                PetType = source.PetType.Name,
                AnimalType = source.AnimalType,
                Age = source.Age,
                Breed = source.Breed,
                IsVaccinated = source.IsVaccinated,
                Gender = source.Gender?.Name,
                IsExotic = source.IsExotic,

                AccessoryType = source.AccessoryType,
                Materials = source.Materials,

                AdoptionRequirements = source.AdoptionRequirements,
                HealthStatus = source.HealthStatus,
                IsSterilized = source.IsSterilized,
            };

        public static RealEstateDetailsResponse ToResponse(this RealEstateDetails source)
            => new()
            {
                Title = source.Title,
                Price = source.Price,
                Description = source.Description,

                LotSizeInSquareMeters = source.LotSizeInSquareMeters,
                ConstructedSquareFootage = source.ConstructedSquareFootage,
                Bedrooms = source.Bedrooms,
                Bathrooms = source.Bathrooms,
                ParkingSpaces = source.ParkingSpaces,
                PropertyType = source.PropertyType.Name,
                Condition = source.Condition?.Name,
                Floors = source.Floors,
                AdditionalFeatures = source.AdditionalFeatures
            };

        public static OthersDetailsResponse ToResponse(this OthersDetails source)
            => new()
            {
                Title = source.Title,
                Price = source.Price,
                Description = source.Description,

                Condition = source.Condition?.Name,
                Brand = source.Brand,
                Features = source.Features,
                WarrantyUntil = source.WarrantyUntil
            };
    }
}
