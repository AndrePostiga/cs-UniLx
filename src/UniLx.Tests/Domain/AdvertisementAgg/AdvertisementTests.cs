using System.Reflection;
using UniLx.Domain.Entities.AccountAgg;
using UniLx.Domain.Entities.AdvertisementAgg;
using UniLx.Domain.Entities.AdvertisementAgg.Enumerations;
using UniLx.Domain.Entities.Seedwork;
using UniLx.Domain.Exceptions;
using Xunit;

namespace UniLx.Tests.Domain.AdvertisementAgg
{
    public class AdvertisementTests
    {
        [Fact]
        public void Advertisement_Protected_Default_Constructor_Should_Work()
        {
            // Act
            var constructor = typeof(Advertisement).GetConstructor(
                BindingFlags.NonPublic | BindingFlags.Instance,
                null,
                Type.EmptyTypes,
                null);

            var advertisement = (Advertisement)constructor!.Invoke(null);

            // Assert
            Assert.NotNull(advertisement);
            Assert.Null(advertisement.Details);
            Assert.Null(advertisement.OwnerId);
            Assert.Null(advertisement.CategoryId);
            Assert.Null(advertisement.CategoryName);
        }

        [Fact]
        public void Advertisement_Should_Create_Valid_Instance()
        {
            // Arrange
            var category = Category.CreateNewCategory("real_estate", "Rentals", "Apartamentos Para Alugar", "Residential rental properties.");
            var address = Address.CreateAddress(country: "BR", state: "RJ", city: "Rio de Janeiro", zipCode: "12345");
            var account = new Account("Test User", "test@example.com", "15480563084", "Test account description", Guid.NewGuid().ToString());
            var details = new TestDetailsStub("Valid Title", "Valid Description", 100);

            // Act
            var advertisement = new Advertisement(
                "real_estate",
                category,
                details,
                DateTime.UtcNow.AddDays(30),
                address,
                account);

            // Assert
            Assert.NotNull(advertisement);
            Assert.Equal("real_estate", advertisement.Type.Name);
            Assert.Equal(category.Id, advertisement.CategoryId);
            Assert.Equal(category.Name, advertisement.CategoryName);
            Assert.Equal(address, advertisement.Address);
            Assert.Equal(account.Id, advertisement.OwnerId);
            Assert.NotNull(advertisement.Details);
            Assert.Equal(AdvertisementStatus.Created, advertisement.Status);
        }

        [Fact]
        public void Advertisement_Should_Throw_When_Category_Is_Invalid()
        {
            // Arrange
            var category = Category.CreateNewCategory("Electronics", "products", "Eletrônicos", "Consumer electronics.");
            var address = Address.CreateAddress(country: "BR", state: "RJ", city: "Rio de Janeiro", zipCode: "12345");
            var account = new Account("Test User", "test@example.com", "15480563084", "Test account description", Guid.NewGuid().ToString());
            var details = new TestDetailsStub("Valid Title", "Valid Description", 100);

            // Act & Assert
            Assert.Throws<DomainException>(() =>
                new Advertisement(
                    "real_estate",
                    category,
                    details,
                    DateTime.UtcNow.AddDays(30),
                    address,
                    account));
        }

        [Fact]
        public void Advertisement_Should_Throw_When_Expiration_Is_Invalid()
        {
            // Arrange
            var category = Category.CreateNewCategory("real_estate", "Rentals", "Apartamentos Para Alugar", "Residential rental properties.");
            var address = Address.CreateAddress(country: "BR", state: "RJ", city: "Rio de Janeiro", zipCode: "12345");
            var account = new Account("Test User", "test@example.com", "15480563084", "Test account description", Guid.NewGuid().ToString());
            var details = new TestDetailsStub("Valid Title", "Valid Description", 100);

            // Act & Assert
            Assert.Throws<DomainException>(() =>
                new Advertisement(
                    "real_estate",
                    category,
                    details,
                    DateTime.UtcNow.AddDays(-1),
                    address,
                    account));

            Assert.Throws<DomainException>(() =>
                new Advertisement(
                    "real_estate",
                    category,
                    details,
                    DateTime.UtcNow.AddDays(91),
                    address,
                    account));
        }

        [Fact]
        public void Advertisement_Should_Throw_When_Address_Is_Invalid()
        {
            // Arrange
            var category = Category.CreateNewCategory("services", "HomeCleaning", "Limpeza Doméstica", "Residential cleaning services.");
            var invalidAddress = Address.CreateAddress(country: "US", state: "NY", city: "New York", zipCode: "12345");
            var account = new Account("Test User", "test@example.com", "15480563084", "Test account description", Guid.NewGuid().ToString());
            var details = new TestDetailsStub("Valid Title", "Valid Description", 100);

            // Act & Assert
            Assert.Throws<DomainException>(() =>
                new Advertisement(
                    "real_estate",
                    category,
                    details,
                    DateTime.UtcNow.AddDays(30),
                    invalidAddress,
                    account));
        }

        [Fact]
        public void Advertisement_Should_Set_Default_Expiration_Date_When_None_Is_Provided()
        {
            // Arrange
            var category = Category.CreateNewCategory("real_estate", "Rentals", "Apartamentos Para Alugar", "Residential rental properties.");
            var address = Address.CreateAddress(country: "BR", state: "RJ", city: "Rio de Janeiro", zipCode: "12345");
            var account = new Account("Test User", "test@example.com", "15480563084", "Test account description", Guid.NewGuid().ToString());
            var details = new TestDetailsStub("Valid Title", "Valid Description", 100);

            // Act
            var advertisement = new Advertisement(
                "real_estate",
                category,
                details,
                null, // No expiration date provided
                address,
                account);

            // Assert
            Assert.Equal(DateTime.UtcNow.AddDays(30).Date, advertisement.ExpiresAt.Date);
        }

        [Fact]
        public void Advertisement_Status_Should_Be_Created_After_Initialization()
        {
            // Arrange
            var category = Category.CreateNewCategory("real_estate", "Rentals", "Apartamentos Para Alugar", "Residential rental properties.");
            var address = Address.CreateAddress(country: "BR", state: "RJ", city: "Rio de Janeiro", zipCode: "12345");
            var account = new Account("Test User", "test@example.com", "15480563084", "Test account description", Guid.NewGuid().ToString());
            var details = new TestDetailsStub("Valid Title", "Valid Description", 100);

            // Act
            var advertisement = new Advertisement(
                "real_estate",
                category,
                details,
                DateTime.UtcNow.AddDays(30),
                address,
                account);

            // Assert
            Assert.Equal(AdvertisementStatus.Created, advertisement.Status);
        }
    }

    // Mock Details Implementation for Testing
    public class TestDetailsStub : Details
    {
        protected override AdvertisementType Type => AdvertisementType.RealEstate;

        public TestDetailsStub(string title, string description, int price) : base(title, description, price) { }
    }
}
