using UniLx.Application.Usecases.Advertisements.Commands.CreateAdvertisement.DetailsCommands;
using UniLx.Application.Usecases.Advertisements.Commands.CreateAdvertisement.DetailsCommands.UniLx.Application.Usecases.Advertisements.Commands.CreateAdvertisement.DetailsCommands;
using UniLx.Domain.Entities.AdvertisementAgg;
using UniLx.Domain.Entities.AdvertisementAgg.Enumerations;
using UniLx.Domain.Entities.AdvertisementAgg.SpecificDetails;
using UniLx.Domain.Exceptions;

namespace UniLx.Application.Usecases.Advertisements.Commands.CreateAdvertisement.Factories
{
    internal static class DetailsFactory
    {
        public static Details ToDetails(this CreateAdvertisementCommand command)
        {
            var type = AdvertisementType.FromName(command.Type, true);

            return (int)type switch
            {
                var e when e.Equals(AdvertisementType.Beauty) => CreateSpecificDetails(command.BeautyDetails!),
                var e when e.Equals(AdvertisementType.Events) => CreateSpecificDetails(command.EventsDetails!),
                var e when e.Equals(AdvertisementType.Electronics) => CreateSpecificDetails(command.ElectronicsDetails!),
                var e when e.Equals(AdvertisementType.Fashion) => CreateSpecificDetails(command.FashionDetails!),
                var e when e.Equals(AdvertisementType.JobOpportunities) => CreateSpecificDetails(command.JobOpportunitiesDetails!),
                var e when e.Equals(AdvertisementType.Pets) => CreateSpecificDetails(command.PetDetails!),
                var e when e.Equals(AdvertisementType.RealEstate) => CreateSpecificDetails(command.RealStateDetails!),
                var e when e.Equals(AdvertisementType.Others) => CreateSpecificDetails(command.OthersDetails!),
                _ => throw new DomainException($"Cannot create details from {command.Type}."),
            };
        }

        private static EventsDetails CreateSpecificDetails(CreateEventsDetailsCommand command)
        {
            return new EventsDetails(command.Title!,
                command.Description,
                command.Price,
                command.EventType!,
                command.EventDate.GetValueOrDefault(),
                command.Organizer,
                command.AgeRestriction,
                command.DressCode,
                command.Highlights,
                command.IsOnline.GetValueOrDefault(),
                command.ContactInformation?.ToContactInformation());
        }

        private static BeautyDetails CreateSpecificDetails(CreateBeautyDetailsCommand command)
        {
            return new BeautyDetails(command.Title,
                command.Description,
                command.Price,
                command.ProductType,
                command.Brand,
                command.SkinType,
                command.ExpirationDate,
                command.Ingredients,
                command.IsOrganic);
        }

        private static ElectronicsDetails CreateSpecificDetails(CreateElectronicsDetailsCommand command)
        {
            return new ElectronicsDetails(
                command.Title!,
                command.Description,
                command.Price,
                command.ProductType!,
                command.Brand!,
                command.Model,
                command.StorageCapacity,
                command.Memory,
                command.Processor,
                command.GraphicsCard,
                command.BatteryLife,
                command.WarrantyUntil,
                command.Features,
                command.Condition!,
                command.IncludesOriginalBox,
                command.Accessories);
        }

        private static FashionDetails CreateSpecificDetails(CreateFashionDetailsCommand command)
        {
            return new FashionDetails(
                command.Title!,
                command.Description,
                command.Price,
                command.ClothingType!,
                command.Brand!,
                command.Sizes ?? [],
                command.Gender!,
                command.Colors,
                command.Materials,
                command.Features,
                command.Designer,
                command.IsHandmade.GetValueOrDefault(),
                command.ReleaseDate,
                command.IsSustainable.GetValueOrDefault());
        }

        private static JobOpportunitiesDetails CreateSpecificDetails(CreateJobOpportunitiesDetailsCommand command)
        {
            return new JobOpportunitiesDetails(
                command.Title!,
                command.Description!,
                command.Position!,
                command.Company!,
                command.Salary,
                command.IsSalaryDisclosed,
                command.WorkLocation!,
                command.EmploymentType!,
                command.ExperienceLevel,
                command.Skills,
                command.Benefits,
                command.RelocationHelp,
                command.ApplicationDeadline,
                command.ContactInformation?.ToContactInformation());
        }

        private static PetDetails CreateSpecificDetails(CreatePetDetailsCommand command)
        {
            return new PetDetails(
                command.Title!,
                command.Description,
                command.Price,
                command.PetType!,
                command.AnimalType!,
                command.Age,
                command.Breed,
                command.IsVaccinated,
                command.Gender,
                command.IsExotic,
                command.AccessoryType,
                command.Materials,
                command.AdoptionRequirements,
                command.HealthStatus,
                command.IsSterilized);
        }

        private static RealEstateDetails CreateSpecificDetails(CreateRealEstateDetailsCommand command)
        {
            return new RealEstateDetails(
                command.Title!,
                command.Description,
                command.Price.GetValueOrDefault(),
                command.LotSizeInSquareMeters,
                command.PropertyType,
                command.Condition,
                command.ConstructedSquareFootage,
                command.Bedrooms,
                command.Bathrooms,
                command.ParkingSpaces,
                command.Floors,
                command.AdditionalFeatures
            );
        }

        private static OthersDetails CreateSpecificDetails(CreateOthersDetailsCommand command)
        {
            return new OthersDetails(
                command.Title,
                command.Description,
                command.Price,
                command.Condition,
                command.Brand,
                command.Features,
                command.WarrantyUntil
            );
        }
    }
}
