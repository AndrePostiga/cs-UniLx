using UniLx.Application.Usecases.Accounts.Commands.CreateAccount.Models;

namespace UniLx.Application.Usecases.Accounts.Commands.CreateAccount.Mappers
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
