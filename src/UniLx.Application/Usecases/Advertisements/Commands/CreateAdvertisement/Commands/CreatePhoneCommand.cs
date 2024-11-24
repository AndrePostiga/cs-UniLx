using FluentValidation;

namespace UniLx.Application.Usecases.Advertisements.Commands.CreateAdvertisement.Commands
{
    public class CreatePhoneCommand
    {
        public string CountryCode { get; set; } = "55";
        public string AreaCode { get; set; }
        public string Number { get; set; }

        public CreatePhoneCommand(string countryCode, string areaCode, string number)
        {
            CountryCode = countryCode;
            AreaCode = areaCode;
            Number = number;
        }
    }

    public class CreatePhoneCommandValidator : AbstractValidator<CreatePhoneCommand>
    {
        public CreatePhoneCommandValidator()
        {
            RuleFor(x => x.CountryCode)
                .NotEmpty().WithMessage("CountryCode is required.")
                .Matches(@"^\d{1,3}$").WithMessage("CountryCode must be a numeric value.")
                .Equal("55").WithMessage("Only country code 55 is allowed.");

            RuleFor(x => x.AreaCode)
                .NotEmpty().WithMessage("AreaCode is required.")
                .Matches(@"^\d{2}$").WithMessage("AreaCode must be a two-digit numeric value.");

            RuleFor(x => x.Number)
                .NotEmpty().WithMessage("Number is required.")
                .Matches(@"^(9\d{8}|\d{8})$").WithMessage("Number must be in the format 9XXXXXXXX or XXXXXXXX.");
        }
    }
}
