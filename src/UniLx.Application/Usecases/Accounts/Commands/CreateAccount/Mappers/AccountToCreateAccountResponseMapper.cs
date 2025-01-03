using UniLx.Application.Usecases.Accounts.Commands.CreateAccount.Models;
using UniLx.Application.Usecases.SharedModels.Mappers;
using UniLx.Domain.Entities.AccountAgg;

namespace UniLx.Application.Usecases.Accounts.Commands.CreateAccount.Mappers
{
    internal static class AccountToCreateAccountResponseMapper
    {
        public static CreateAccountResponse ToResponse(this Account source)
            => new(Id: source.Id,
                Name: source.Name,
                Cpf: source.Cpf.Value,
                Description: source.Description,
                Email: source.Email.Value,
                ProfilePictureUrl: source.ProfilePictureUrl,
                Rating: source.Rating.ToResponse(),
                Advertisements: [..(source.AdvertisementIds ?? [])],
                InterestedAdvertisements: [..(source.InterestAdvertisementIds ?? [])],
                CreatedAt: source.CreatedAt!);
    }
}
