using FluentValidation;

namespace UniLx.Application.Usecases.Advertisements.Commands.CreateAdvertisement.Commands
{
    public class CreateContactInformationCommand
    {
        public CreatePhoneCommand? Phone { get; set; }
        public string? Email { get; set; }
        public string? Website { get; set; }

        public CreateContactInformationCommand(CreatePhoneCommand? phone, string? email, string? website)
        {
            Phone = phone;
            Email = email;
            Website = website;
        }
    }

    public class CreateContactInformationCommandValidator : AbstractValidator<CreateContactInformationCommand>
    {
        public CreateContactInformationCommandValidator()
        {
            RuleFor(x => x.Phone)
                .SetValidator(new CreatePhoneCommandValidator()!)
                .When(x => x.Phone != null);

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Invalid email format.")
                .When(x => !string.IsNullOrWhiteSpace(x.Email));

            RuleFor(x => x.Website)
                .Must(BeAValidUrl).WithMessage("Invalid website URL format.")
                .When(x => !string.IsNullOrWhiteSpace(x.Website));

            RuleFor(x => x)
                .Must(x => x.Phone != null || !string.IsNullOrWhiteSpace(x.Email))
                .WithMessage("At least one contact method (Phone or Email) must be provided.");
        }

        private bool BeAValidUrl(string? url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out _);
        }
    }
}
