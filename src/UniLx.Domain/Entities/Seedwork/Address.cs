using Ardalis.SmartEnum;
using Dawn;
using UniLx.Domain.Exceptions;
using UniLx.Shared;

namespace UniLx.Domain.Entities.Seedwork
{
    public class AddressType : SmartEnum<AddressType>
    {
        public static readonly AddressType Coordinates = new AddressType("Coordinates", 1);
        public static readonly AddressType FullAddress = new AddressType("FullAddress", 2);
        public static readonly AddressType Both = new AddressType("Both", 3);

        private AddressType(string name, int value) : base(name, value) { }
    }

    public class Address
    {
        public bool HasCordinates => Type == AddressType.Coordinates || Type == AddressType.Both;

        public double? Latitude { get; private set; }
        public double? Longitude { get; private set; }
        public string? Country { get; private set; }
        public string? State { get; private set; }
        public string? City { get; private set; }
        public string? Neighborhood { get; private set; }
        public string? ZipCode { get; private set; }
        public string? Street { get; private set; }
        public string? Number { get; private set; }
        public string? Complement { get; private set; }
        public AddressType Type { get; private set; }

        private Address() { }

        private Address(AddressType type, double? latitude = null, double? longitude = null,
                        string? country = null, string? state = null, string? city = null,
                        string? neighborhood = null, string? zipCode = null,
                        string? street = null, string? number = null, string? complement = null)
        {
            Type = type;
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
        
        public static Address CreateAddress(double? latitude = null, double? longitude = null,
                                                 string? country = null, string? state = null,
                                                 string? city = null, string? neighborhood = null,
                                                 string? zipCode = null, string? street = null,
                                                 string? number = null, string? complement = null)
        {
            if (latitude.HasValue && longitude.HasValue)
            {
                return !string.IsNullOrEmpty(country) || !string.IsNullOrEmpty(state) ||
                       !string.IsNullOrEmpty(city) || !string.IsNullOrEmpty(zipCode)
                    ? CreateAddressWithCoordinatesAndFullAddress(latitude.Value, longitude.Value, country, state, city, zipCode, neighborhood, street, number, complement)
                    : CreateCoordinates(latitude.Value, longitude.Value);
            }

            if (!string.IsNullOrEmpty(country) && !string.IsNullOrEmpty(state) &&
                !string.IsNullOrEmpty(city) && !string.IsNullOrEmpty(zipCode))
            {
                return CreateFullAddress(country, state, city, zipCode, neighborhood, street, number, complement);
            }

            throw new DomainException("Invalid address: Must contain either valid coordinates or a full address.");
        }

        
        private static Address CreateCoordinates(double latitude, double longitude)
        {
            DomainException.ThrowIf(latitude < -90 || latitude > 90, "Latitude must be between -90 and 90.");
            DomainException.ThrowIf(longitude < -180 || longitude > 180, "Longitude must be between -180 and 180.");

            return new Address(AddressType.Coordinates, latitude, longitude);
        }

        private static Address CreateFullAddress(string country, string state, string city, string zipCode,
                                                 string? neighborhood = null, string? street = null,
                                                 string? number = null, string? complement = null)
        {            
            country = Guard.Argument(country.OnlyLetters(), nameof(country))
                .NotNull().NotEmpty().NotWhiteSpace()
                .Modify(p => p.WithTrimmedSpaces().ToUpper()).Length(2);

            state = Guard.Argument(state.OnlyLetters(), nameof(state))
                .NotNull().NotEmpty().NotWhiteSpace()
                .Modify(p => p.WithTrimmedSpaces().ToUpper()).Length(2);

            city = Guard.Argument(city, nameof(city))
                .NotNull().NotEmpty().NotWhiteSpace()
                .Modify(p => p.WithTrimmedSpaces());

            zipCode = Guard.Argument(zipCode, nameof(zipCode))
                .NotNull().NotEmpty().NotWhiteSpace();

            return new Address(AddressType.FullAddress, null, null, country, state, city, neighborhood, zipCode, street, number, complement);
        }

        private static Address CreateAddressWithCoordinatesAndFullAddress(double latitude, double longitude,
                                                                          string country, string state, string city,
                                                                          string zipCode, string? neighborhood = null,
                                                                          string? street = null, string? number = null,
                                                                          string? complement = null)
        {            
            DomainException.ThrowIf(latitude < -90 || latitude > 90, "Latitude must be between -90 and 90.");
            DomainException.ThrowIf(longitude < -180 || longitude > 180, "Longitude must be between -180 and 180.");

            country = Guard.Argument(country.OnlyLetters(), nameof(country))
                .NotNull().NotEmpty().NotWhiteSpace()
                .Modify(p => p.WithTrimmedSpaces().ToUpper()).Length(2);

            state = Guard.Argument(state.OnlyLetters(), nameof(state))
                .NotNull().NotEmpty().NotWhiteSpace()
                .Modify(p => p.WithTrimmedSpaces().ToUpper()).Length(2);

            city = Guard.Argument(city, nameof(city))
                .NotNull().NotEmpty().NotWhiteSpace()
                .Modify(p => p.WithTrimmedSpaces());

            zipCode = Guard.Argument(zipCode, nameof(zipCode))
                .NotNull().NotEmpty().NotWhiteSpace();

            return new Address(AddressType.Both, latitude, longitude, country, state, city, neighborhood, zipCode, street, number, complement);
        }
    }
}
