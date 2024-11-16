using FluentValidation;

namespace UniLx.Application.Usecases.Advertisements.Commands.CreateAdvertisement.Commands
{
    public class CreateAddressCommand
    {
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string? Country { get; set; }
        public string? State { get; set; }
        public string? City { get; set; }
        public string? Neighborhood { get; set; }
        public string? ZipCode { get; set; }
        public string? Street { get; set; }
        public string? Number { get; set; }
        public string? Complement { get; set; }

        public CreateAddressCommand(double? latitude = null, double? longitude = null,
                              string? country = null, string? state = null, string? city = null,
                              string? neighborhood = null, string? zipCode = null,
                              string? street = null, string? number = null, string? complement = null)
        {
            Latitude = latitude;
            Longitude = longitude;
            Country = country;
            State = state;
            City = city;
            Neighborhood = neighborhood;
            ZipCode = zipCode;
            Street = street;
            Number = number;
            Complement = complement;
        }

        public class CreateAddressCommandValidator : AbstractValidator<CreateAddressCommand>
        {
            public CreateAddressCommandValidator()
            {
                RuleFor(x => x)
                    .Must(x => RequestHasLatLong(x) || RequestHasFullAddress(x))
                    .WithMessage("You must provide both latitude and longitude, a complete address, or both.");

                RuleFor(x => x.Latitude)
                    .InclusiveBetween(-90, 90).WithMessage("Latitude must be between -90 and 90.")
                    .When(x => x.Latitude.HasValue);

                RuleFor(x => x.Longitude)
                    .InclusiveBetween(-180, 180).WithMessage("Longitude must be between -180 and 180.")
                    .When(x => x.Longitude.HasValue);

                RuleFor(x => x)
                    .Must(x => !(x.Latitude.HasValue ^ x.Longitude.HasValue))
                    .WithMessage("Both latitude and longitude must be provided together if one of them is specified.");


                RuleFor(x => x.Country)
                    .NotEmpty().WithMessage("Country is required when providing a full address.")
                    .Length(2).WithMessage("Country code must be exactly 2 characters.")
                    .Matches("^[A-Za-z]+$").WithMessage("Country code must contain only letters.")
                    .When(x => !string.IsNullOrEmpty(x.Country) || !string.IsNullOrEmpty(x.State) ||
                               !string.IsNullOrEmpty(x.City) || !string.IsNullOrEmpty(x.ZipCode));

                RuleFor(x => x.State)
                    .NotEmpty().WithMessage("State is required when providing a full address.")
                    .Length(2).WithMessage("State code must be exactly 2 characters.")
                    .Matches("^[A-Za-z]+$").WithMessage("State code must contain only letters.")
                    .When(x => !string.IsNullOrEmpty(x.Country) || !string.IsNullOrEmpty(x.State) ||
                               !string.IsNullOrEmpty(x.City) || !string.IsNullOrEmpty(x.ZipCode));

                RuleFor(x => x.City)
                    .NotEmpty().WithMessage("City is required when providing a full address.")
                    .When(x => !string.IsNullOrEmpty(x.Country) || !string.IsNullOrEmpty(x.State) ||
                               !string.IsNullOrEmpty(x.City) || !string.IsNullOrEmpty(x.ZipCode));

                RuleFor(x => x.ZipCode)
                    .NotEmpty().WithMessage("Zip code is required when providing a full address.")
                    .When(x => !string.IsNullOrEmpty(x.Country) || !string.IsNullOrEmpty(x.State) ||
                               !string.IsNullOrEmpty(x.City) || !string.IsNullOrEmpty(x.ZipCode));

                RuleFor(x => x.Neighborhood)
                    .MaximumLength(100).WithMessage("Neighborhood must not exceed 100 characters.")
                    .When(x => !string.IsNullOrEmpty(x.Neighborhood));

                RuleFor(x => x.Street)
                    .MaximumLength(100).WithMessage("Street must not exceed 100 characters.")
                    .When(x => !string.IsNullOrEmpty(x.Street));

                RuleFor(x => x.Number)
                    .MaximumLength(10).WithMessage("Number must not exceed 50 characters.")
                    .When(x => !string.IsNullOrEmpty(x.Number));

                RuleFor(x => x.Complement)
                    .MaximumLength(64).WithMessage("Complement must not exceed 64 characters.")
                    .When(x => !string.IsNullOrEmpty(x.Complement));
            }

            private static bool RequestHasFullAddress(CreateAddressCommand addressCommand)
            {
                return !string.IsNullOrEmpty(addressCommand.Country) && 
                    !string.IsNullOrEmpty(addressCommand.State) && 
                    !string.IsNullOrEmpty(addressCommand.City) && 
                    !string.IsNullOrEmpty(addressCommand.ZipCode);
            }

            private static bool RequestHasLatLong(CreateAddressCommand addressCommand)
            {
                return addressCommand.Latitude.HasValue && addressCommand.Longitude.HasValue;
            }
        }
    }
}
