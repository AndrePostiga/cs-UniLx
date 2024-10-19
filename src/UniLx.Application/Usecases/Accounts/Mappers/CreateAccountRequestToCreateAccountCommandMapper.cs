using UniLx.Application.Usecases.Accounts.Commands.CreateAccount;
using UniLx.Application.Usecases.Accounts.Requests;

namespace UniLx.Application.Usecases.Accounts.Mappers
{
    public static class CreateAccountRequestToCreateAccountCommandMapper
    {
        public static CreateAccountCommand ToCommand(this CreateAccountRequest source)
            => new(name: source.Name,
                cpf: source.Cpf,
                email: source.Email,
                description: source.Description,
                profilePicturePath: source.ProfilePicturePath);
    }
}
