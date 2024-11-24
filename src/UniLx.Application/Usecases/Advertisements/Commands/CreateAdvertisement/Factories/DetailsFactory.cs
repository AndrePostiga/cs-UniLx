using UniLx.Application.Usecases.Advertisements.Commands.CreateAdvertisement.DetailsCommands;
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
                command.ContactInformation.ToContactInformation());
        }

    }
}
