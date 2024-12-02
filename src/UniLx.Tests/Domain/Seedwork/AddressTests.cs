using UniLx.Domain.Entities.Seedwork;
using UniLx.Domain.Exceptions;

namespace UniLx.Tests.Domain.Seedwork
{
    public class AddressTests
    {
        [Fact]
        public void Address_Should_Create_Valid_FullAddress()
        {
            // Arrange
            var country = "BR";
            var state = "RJ";
            var city = "Rio de Janeiro";
            var neighborhood = "Centro";
            var zipCode = "20000-000";
            var street = "Rua A";
            var number = "100";

            // Act
            var address = Address.CreateAddress(
                null,
                null,
                country,
                state,
                city,
                neighborhood,
                zipCode,
                street,
                number
            );

            // Assert
            Assert.NotNull(address);
            Assert.Equal(country, address.Country);
            Assert.Equal(state, address.State);
            Assert.Equal(city, address.City);
            Assert.Equal(neighborhood, address.Neighborhood);
            Assert.Equal(zipCode, address.ZipCode);
            Assert.Equal(street, address.Street);
            Assert.Equal(number, address.Number);
            Assert.False(address.HasCoordinates);
            Assert.Equal($"{number}, {street}, {neighborhood}, {city}, {state}, {country}, {zipCode}", address.FullAddress);
        }

        [Fact]
        public void Address_Should_Create_With_Coordinates()
        {
            // Arrange
            var latitude = -22.9068;
            var longitude = -43.1729;

            // Act
            var address = Address.CreateAddress(latitude, longitude);

            // Assert
            Assert.NotNull(address);
            Assert.Equal(latitude, address.Latitude);
            Assert.Equal(longitude, address.Longitude);
            Assert.True(address.HasCoordinates);
            Assert.Equal(AddressType.Coordinates, address.Type);
        }

        [Fact]
        public void Address_Should_Throw_On_Invalid_Coordinates()
        {
            // Arrange
            var invalidLatitude = 100.0; // Invalid latitude
            var invalidLongitude = 200.0; // Invalid longitude

            // Act & Assert
            Assert.Throws<DomainException>(() =>
            {
                Address.CreateAddress(invalidLatitude, invalidLongitude);
            });
        }

        [Fact]
        public void Address_Should_Create_Valid_Address_With_Coordinates_And_FullAddress()
        {
            // Arrange
            var latitude = -22.9068;
            var longitude = -43.1729;
            var country = "BR";
            var state = "RJ";
            var city = "Rio de Janeiro";
            var neighborhood = "Centro";
            var zipCode = "20000-000";
            var street = "Rua A";
            var number = "100";

            // Act
            var address = Address.CreateAddress(
                latitude,
                longitude,
                country,
                state,
                city,
                neighborhood,
                zipCode,
                street,
                number
            );

            // Assert
            Assert.NotNull(address);
            Assert.Equal(latitude, address.Latitude);
            Assert.Equal(longitude, address.Longitude);
            Assert.Equal(country, address.Country);
            Assert.Equal(state, address.State);
            Assert.Equal(city, address.City);
            Assert.True(address.HasCoordinates);
            Assert.Equal(AddressType.Both, address.Type);
        }

        [Fact]
        public void Address_Should_Throw_When_No_Valid_FullAddress_Or_Coordinates()
        {
            // Arrange
            var country = ""; // Invalid country
            var state = "RJ";
            var city = "Rio de Janeiro";
            var zipCode = "20000-000";

            // Act & Assert
            Assert.Throws<DomainException>(() =>
            {
                Address.CreateAddress(
                    null,
                    null,
                    country,
                    state,
                    city,
                    null,
                    zipCode
                );
            });
        }

        [Fact]
        public void Address_Should_Throw_On_Invalid_Country()
        {
            // Arrange
            var invalidCountry = "BRA"; // Too long
            var state = "RJ";
            var city = "Rio de Janeiro";
            var zipCode = "20000-000";

            // Act & Assert
            Assert.Throws<DomainException>(() =>
            {
                Address.CreateAddress(
                    null,
                    null,
                    invalidCountry,
                    state,
                    city,
                    null,
                    zipCode
                );
            });
        }

        [Fact]
        public void Address_Should_Throw_On_Invalid_State()
        {
            // Arrange
            var country = "BR";
            var invalidState = "RJS"; // Too long
            var city = "Rio de Janeiro";
            var zipCode = "20000-000";

            // Act & Assert
            Assert.Throws<DomainException>(() =>
            {
                Address.CreateAddress(
                    null,
                    null,
                    country,
                    invalidState,
                    city,
                    null,
                    zipCode
                );
            });
        }

        [Fact]
        public void Address_Should_Throw_On_Invalid_ZipCode()
        {
            // Arrange
            var country = "BR";
            var state = "RJ";
            var city = "Rio de Janeiro";
            var invalidZipCode = ""; // Empty zip code

            // Act & Assert
            Assert.Throws<DomainException>(() =>
            {
                Address.CreateAddress(
                    null,
                    null,
                    country,
                    state,
                    city,
                    null,
                    invalidZipCode
                );
            });
        }
    }
}
