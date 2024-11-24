using UniLx.Application.Usecases.Advertisements.Commands.CreateAdvertisement.Commands;
using UniLx.Application.Usecases.Advertisements.Commands.CreateAdvertisement.DetailsCommands;
using UniLx.Application.Usecases.Advertisements.Commands.CreateAdvertisement.Models.Request;
using UniLx.Application.Usecases.Advertisements.Commands.CreateAdvertisement.Models.Request.DetailsRequest;
using UniLx.Application.Usecases.SharedModels.Requests;

namespace UniLx.Application.Usecases.Advertisements.Commands.CreateAdvertisement.Mappers
{
    public static class CreateAdvertisementRequestToCommandMapper
    {
        public static CreateAdvertisementCommand ToCommand(this CreateAdvertisementRequest source, string accountId)
            => new (
                accountId: accountId,
                type: source.Type,
                subCategory: source.SubCategory,
                expiresAt: source.ExpiresAt,
                address: source.Address?.ToCommand(),
                beautyDetails: source.BeautyDetails?.ToCommand(),
                eventDetails: source.EventsDetails?.ToCommand(),
                electronicsDetails: source.ElectronicsDetails?.ToCommand(),
                fashionDetails: source.FashionDetails?.ToCommand(), 
                jobOpportunitiesDetails: source.JobOpportunitiesDetails?.ToCommand());

        public static CreateAddressCommand ToCommand(this AddressRequest source)
            => new (
                latitude: source.Latitude,
                longitude: source.Longitude,
                country: source.Country,
                state: source.State,
                city: source.City,
                neighborhood: source.Neighborhood,
                zipCode: source.ZipCode,
                street: source.Street,
                number: source.Number,
                complement: source.Complement
            );

        public static CreateBeautyDetailsCommand ToCommand(this BeautyDetailsRequest source)
            => new (
                source.Title,
                source.Description,
                source.Price ?? 0,
                source.ProductType,
                source.Brand,
                source.SkinType,
                source.ExpirationDate,
                source.Ingredients,
                source.IsOrganic ?? false
            );

        public static CreateElectronicsDetailsCommand ToCommand(this ElectronicsDetailsRequest source)
            => new(
                source.Title,
                source.Description,
                source.Price ?? 0,
                source.ProductType,
                source.Brand,
                source.Model,
                source.StorageCapacity,
                source.Memory,
                source.Processor,
                source.GraphicsCard,
                source.BatteryLife,
                source.WarrantyUntil,
                source.Features,
                source.Condition,
                source.IncludesOriginalBox ?? false,
                source.Accessories
            );

        public static CreateEventsDetailsCommand ToCommand(this EventDetailsRequest source)
            => new(
                source.Title,
                source.Description,
                source.Price ?? 0,
                source.EventType,
                source.EventDate,
                source.Organizer,
                source.AgeRestriction,
                source.DressCode,
                source.Highlights,
                source.IsOnline ?? false,
                source.ContactInformation?.ToCommand()
            );

        public static CreateFashionDetailsCommand ToCommand(this FashionDetailsRequest source)
            => new(
                source.Title,
                source.Description,
                source.Price ?? 0,
                source.ClothingType,
                source.Brand,
                source.Sizes,
                source.Gender,
                source.Colors,
                source.Materials,
                source.Features,
                source.Designer,
                source.IsHandmade,
                source.ReleaseDate,
                source.IsSustainable
            );

        public static CreateJobOpportunitiesDetailsCommand ToCommand(this JobOpportunitiesDetailsRequest source)
            => new(
                source.Title,
                source.Description,
                source.Position!,
                source.Company!,
                source.Salary,
                source.IsSalaryDisclosed ?? false,
                source.WorkLocation!,
                source.EmploymentType!,
                source.ExperienceLevel,
                source.Skills,
                source.Benefits,
                source.RelocationHelp ?? false,
                source.ApplicationDeadline,
                source.ContactInformation?.ToCommand());

        public static CreateContactInformationCommand ToCommand(this ContactInformationRequest source)
            => new(source.Phone?.ToCommand(), source.Email, source.Website);

        public static CreatePhoneCommand ToCommand(this PhoneRequest source)
            => new(source.CountryCode, source.AreaCode, source.Number);
    }
}
