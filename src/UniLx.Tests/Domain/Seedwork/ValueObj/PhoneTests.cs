using System.Reflection;
using UniLx.Domain.Entities.Seedwork.ValueObj;
using UniLx.Domain.Exceptions;

namespace UniLx.Tests.Domain.Seedwork.ValueObj
{
    public class PhoneTests
    {
        [Fact]
        public void Phone_Private_Default_Constructor_Should_Work()
        {
            // Act
            var privateConstructor = typeof(Phone).GetConstructor(
                BindingFlags.NonPublic | BindingFlags.Instance,
                null,
                Type.EmptyTypes,
                null);

            var phone = (Phone)privateConstructor!.Invoke(null);

            // Assert
            Assert.NotNull(phone);
            Assert.Null(phone.CountryCode);
            Assert.Null(phone.AreaCode);
            Assert.Null(phone.Number);
        }

        [Fact]
        public void Phone_Should_Create_Valid_Phone()
        {
            // Arrange
            var countryCode = "55";
            var areaCode = "21";
            var number = "987654321";

            // Act
            var phone = new Phone(countryCode, areaCode, number);

            // Assert
            Assert.NotNull(phone);
            Assert.Equal(countryCode, phone.CountryCode);
            Assert.Equal(areaCode, phone.AreaCode);
            Assert.Equal(number, phone.Number);
            Assert.Equal($"+{countryCode} ({areaCode}) {number}", phone.ToString());
        }

        [Fact]
        public void Phone_Should_Throw_On_Empty_CountryCode()
        {
            // Arrange
            var areaCode = "21";
            var number = "987654321";

            // Act & Assert
            Assert.Throws<DomainException>(() =>
            {
                new Phone("", areaCode, number);
            });
        }

        [Fact]
        public void Phone_Should_Throw_On_Invalid_CountryCode()
        {
            // Arrange
            var invalidCountryCode = "1";
            var areaCode = "21";
            var number = "987654321";

            // Act & Assert
            Assert.Throws<DomainException>(() =>
            {
                new Phone(invalidCountryCode, areaCode, number);
            });
        }

        [Fact]
        public void Phone_Should_Throw_On_Invalid_AreaCode()
        {
            // Arrange
            var countryCode = "55";
            var invalidAreaCode = "00";
            var number = "987654321";

            // Act & Assert
            Assert.Throws<DomainException>(() =>
            {
                new Phone(countryCode, invalidAreaCode, number);
            });
        }

        [Fact]
        public void Phone_Should_Throw_On_Invalid_Number()
        {
            // Arrange
            var countryCode = "55";
            var areaCode = "21";
            var invalidNumber = "12345";

            // Act & Assert
            Assert.Throws<DomainException>(() =>
            {
                new Phone(countryCode, areaCode, invalidNumber);
            });
        }
    }
}
