using Dawn;
using UniLx.Shared;

namespace UniLx.Domain.Entities.AccountAgg
{
    public class Address
    {
        public string Country { get; private set; }
        public string State { get; private set; }
        public string City { get; private set; }
        public string? Neighborhood { get; private set; }
        public string ZipCode { get; private set; }
        public string? Street { get; private set; }
        public string? Number { get; private set; }
        public string? Complement { get; private set; }

        public Address(string country,
                       string state,
                       string city,
                       string? neighborhood,
                       string zipCode,
                       string? street,
                       string? number,
                       string? complement)
        {
            Country = Guard.Argument(country.OnlyLetters(), nameof(Country))
                .NotNull()
                .NotEmpty()
                .NotWhiteSpace()
                .Modify(p => p.WithTrimmedSpaces().ToUpper())
                .Length(2);

            State = Guard.Argument(state.OnlyLetters(), nameof(State))
                .NotNull()
                .NotEmpty()
                .NotWhiteSpace()
                .Modify(p => p.WithTrimmedSpaces().ToUpper())
                .Length(2);

            City = Guard.Argument(city, nameof(City))
                .NotNull()
                .NotEmpty()
                .NotWhiteSpace()
                .Modify(p => p.WithTrimmedSpaces());

            ZipCode = Guard.Argument(zipCode, nameof(ZipCode))
                .NotNull()
                .NotEmpty()
                .NotWhiteSpace();

            Neighborhood = neighborhood is not null
            ? Guard.Argument(neighborhood, nameof(Neighborhood))
                .NotEmpty()
                .NotWhiteSpace()
                .Modify(p => p.WithTrimmedSpaces())
            : null;

            Street = street is not null
            ? Guard.Argument(street, nameof(Street))
                .NotEmpty()
                .NotWhiteSpace()
                .Modify(p => p.WithTrimmedSpaces())
            : null;

            Number = number is not null
            ? Guard.Argument(number, nameof(Number))
                .NotEmpty()
                .NotWhiteSpace()
                .Modify(p => p.WithTrimmedSpaces())
            : null;

            Complement = Guard.Argument(complement, nameof(Complement))
                .Modify(p => p?.WithTrimmedSpaces())
                .MaxLength(64);
        }
    }
}
