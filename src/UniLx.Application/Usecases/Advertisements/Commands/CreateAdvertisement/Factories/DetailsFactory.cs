using UniLx.Application.Usecases.Advertisements.Commands.CreateAdvertisement.DetailsCommand;
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

            switch (type)
            {
                case var e when e.Equals(AdvertisementType.Beauty):
                    return CreateBeautyDetails(command.BeautyDetails!);

                default:
                    throw new DomainException($"Cannot create details from {command.Type}.");
            }
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
