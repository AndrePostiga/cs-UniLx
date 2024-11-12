using FluentValidation;
using Microsoft.AspNetCore.Http;
using UniLx.Domain.Entities.AccountAgg.ValueObj;
using UniLx.Shared.Abstractions;

namespace UniLx.Application.Usecases.Accounts.Commands.CreateAccount
{
    public class CreateAccountCommand : ICommand<IResult>
    {
        public string Name { get; set; }

        public string Cpf { get; set; }

        public string Email { get; set; }

        public string? Description { get; set; }

        public CreateAccountCommand(string name, string cpf, string email, string? description)
        {
            Name = name;
            Cpf = cpf;
            Email = email;
            Description = description;
        }        
    }

    public class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
    {
        public CreateAccountCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required.")
                .MaximumLength(100)
                .WithMessage("Name must be 100 characters or less.");

            RuleFor(x => x.Cpf)
                .NotEmpty()
                .WithMessage("CPF is required.")
                .Must(BeAValidCpf)
                .WithMessage("Invalid CPF format.");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required.")
                .EmailAddress()
                .WithMessage("Invalid email format.");

            RuleFor(x => x.Description)
                .MaximumLength(256)
                .WithMessage("Description must be 256 characters or less.");
        }

        private bool BeAValidCpf(string cpf) => CPF.IsValid(cpf);
    }
}
