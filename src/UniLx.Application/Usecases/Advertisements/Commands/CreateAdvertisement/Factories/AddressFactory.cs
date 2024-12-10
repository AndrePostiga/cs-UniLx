using UniLx.Application.Usecases.Advertisements.Commands.CreateAdvertisement.Commands;
using UniLx.Domain.Entities.Seedwork;

namespace UniLx.Application.Usecases.Advertisements.Commands.CreateAdvertisement.Factories
{
    internal static class AddressFactory
    {
        public static Address ToAddress(this CreateAddressCommand command)
            => Address.CreateAddress(command.Latitude,
                command.Longitude,
                command.Country,
                command.State,
                command.City,
                command.Neighborhood,
                command.ZipCode,
                command.Street,
                command.Number,
                command.Complement);
    }
}
