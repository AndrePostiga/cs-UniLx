using Ardalis.SmartEnum;
using UniLx.Domain.Exceptions;
using UniLx.Shared;

namespace UniLx.Domain.Entities.Seedwork
{
    public class AddressType : SmartEnum<AddressType>
    {
        public static readonly AddressType Coordinates = new("Coordinates", 1);
        public static readonly AddressType FullAddress = new("FullAddress", 2);
        public static readonly AddressType Both = new("Both", 3);

        private AddressType(string name, int value) : base(name, value) { }
    }

    public class Address
    {
        public AddressType Type { get; private set; }
        public bool HasCoordinates => Type == AddressType.Coordinates || Type == AddressType.Both;

        public string? FullAddress => string.Join(", ", new[] {Number, Street, Neighborhood, City, State, Country, ZipCode}.Where(part => !string.IsNullOrWhiteSpace(part)));
        public string? PartialAddress => string.Join(", ", new[] { Neighborhood, City, State, Country, ZipCode }.Where(part => !string.IsNullOrWhiteSpace(part)));

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

        private Address() { }

        private Address(AddressType type, double? latitude = null, double? longitude = null,
                        string? country = null, string? state = null, string? city = null,
                        string? neighborhood = null, string? zipCode = null,
                        string? street = null, string? number = null, string? complement = null)
        {
            Type = type;
            Latitude = latitude;
            Longitude = longitude;
            Country = country?.ToUpperInvariant();
            State = state?.ToUpperInvariant();
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
                return CreateWithCoordinates(latitude.Value, longitude.Value, country, state, city, zipCode, neighborhood, street, number, complement);
            }

            if (IsValidFullAddress(country, state, city, zipCode))
            {
                return CreateFullAddress(country!, state!, city!, zipCode!, neighborhood, street, number, complement);
            }

            throw new DomainException("Invalid address: Must contain either valid coordinates or a full address.");
        }

        private static bool IsValidFullAddress(string? country, string? state, string? city, string? zipCode)
        {
            return !string.IsNullOrEmpty(country) && !string.IsNullOrEmpty(state) &&
                   !string.IsNullOrEmpty(city) && !string.IsNullOrEmpty(zipCode);
        }

        private static Address CreateWithCoordinates(double latitude, double longitude,
                                                      string? country = null, string? state = null,
                                                      string? city = null, string? zipCode = null,
                                                      string? neighborhood = null, string? street = null,
                                                      string? number = null, string? complement = null)
        {
            ValidateCoordinates(latitude, longitude);

            return IsValidFullAddress(country, state, city, zipCode)
                ? CreateAddressWithCoordinatesAndFullAddress(latitude, longitude, country!, state!, city!, zipCode!, neighborhood, street, number, complement)
                : CreateCoordinates(latitude, longitude);
        }

        private static Address CreateCoordinates(double latitude, double longitude)
        {
            ValidateCoordinates(latitude, longitude);
            return new Address(AddressType.Coordinates, latitude, longitude);
        }

        private static Address CreateFullAddress(string country, string state, string city, string zipCode,
                                                 string? neighborhood = null, string? street = null,
                                                 string? number = null, string? complement = null)
        {
            ValidateCountry(country);
            ValidateState(state);
            ValidateCity(city);
            ValidateZipCode(zipCode);

            return new Address(AddressType.FullAddress, null, null, country, state, city, neighborhood, zipCode, street, number, complement);
        }

        private static Address CreateAddressWithCoordinatesAndFullAddress(double latitude, double longitude,
                                                                          string country, string state, string city,
                                                                          string zipCode, string? neighborhood = null,
                                                                          string? street = null, string? number = null,
                                                                          string? complement = null)
        {
            ValidateCoordinates(latitude, longitude);
            ValidateCountry(country);
            ValidateState(state);
            ValidateCity(city);
            ValidateZipCode(zipCode);

            return new Address(AddressType.Both, latitude, longitude, country, state, city, neighborhood, zipCode, street, number, complement);
        }

        private static void ValidateCoordinates(double latitude, double longitude)
        {
            DomainException.ThrowIf(latitude < -90 || latitude > 90, "Latitude must be between -90 and 90.");
            DomainException.ThrowIf(longitude < -180 || longitude > 180, "Longitude must be between -180 and 180.");
        }

        private static void ValidateCountry(string country)
        {
            DomainException.ThrowIf(string.IsNullOrWhiteSpace(country), "Country must not be empty.");
            DomainException.ThrowIf(string.IsNullOrWhiteSpace(country.OnlyLetters()), "Country must contain only letters.");
            DomainException.ThrowIf(country.Length != 2, "Country must be exactly 2 characters long.");
        }

        private static void ValidateState(string state)
        {
            DomainException.ThrowIf(string.IsNullOrWhiteSpace(state), "State must not be empty.");
            DomainException.ThrowIf(string.IsNullOrWhiteSpace(state.OnlyLetters()), "State must contain only letters.");
            DomainException.ThrowIf(state.Length != 2, "State must be exactly 2 characters long.");
        }

        private static void ValidateCity(string city)
        {
            DomainException.ThrowIf(string.IsNullOrWhiteSpace(city), "City must not be empty.");
        }

        private static void ValidateZipCode(string zipCode)
        {
            DomainException.ThrowIf(string.IsNullOrWhiteSpace(zipCode), "ZipCode must not be empty.");
        }
    }
}
