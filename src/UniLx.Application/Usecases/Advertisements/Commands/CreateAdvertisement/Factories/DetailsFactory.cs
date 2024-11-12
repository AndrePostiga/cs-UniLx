using UniLx.Application.Usecases.Advertisements.Commands.CreateAdvertisement.DetailsCommand;
using UniLx.Domain.Entities.AdvertisementAgg;
using UniLx.Domain.Entities.AdvertisementAgg.Enumerations;
using UniLx.Domain.Entities.AdvertisementAgg.SpecificDetails;
using UniLx.Domain.Entities.Seedwork.ValueObj;
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
                var e when e.Equals(AdvertisementType.Beauty) => CreateBeautyDetails(command.BeautyDetails!),
                var e when e.Equals(AdvertisementType.Events) => CreateEventDetails(command.EventsDetails!),
                _ => throw new DomainException($"Cannot create details from {command.Type}."),
            };
        }

        private static EventsDetails CreateEventDetails(CreateEventDetailsCommand command)
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

        private static BeautyDetails CreateBeautyDetails(CreateBeautyDetailsCommand command)
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

    }
}
