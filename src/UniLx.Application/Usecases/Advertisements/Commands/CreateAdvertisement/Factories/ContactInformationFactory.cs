using UniLx.Application.Usecases.Advertisements.Commands.CreateAdvertisement.Commands;
using UniLx.Domain.Entities.Seedwork.ValueObj;

namespace UniLx.Application.Usecases.Advertisements.Commands.CreateAdvertisement.Factories
{
    internal static class ContactInformationFactory
    {
        public static ContactInformation ToContactInformation(this CreateContactInformationCommand command)
            => new(command.Phone?.ToPhone(), 
                string.IsNullOrWhiteSpace(command.Email) ? null : new Email(command.Email), 
                command.Website);

        public static Phone ToPhone(this CreatePhoneCommand command)
            => new(command.CountryCode, command.AreaCode, command.Number);
    }
}
