using Bogus.DataSets;
using System.Reflection;
using UniLx.Domain.Entities.AccountAgg;
using UniLx.Domain.Entities.AdvertisementAgg;
using UniLx.Domain.Entities.AdvertisementAgg.Enumerations;
using UniLx.Domain.Entities.ChatAgg;
using UniLx.Domain.Entities.Seedwork;
using UniLx.Domain.Exceptions;

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
            var address = UniLx.Domain.Entities.Seedwork.Address.CreateAddress(country: "BR", state: "RJ", city: "Rio de Janeiro", zipCode: "12345");
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
            Assert.Equal(AdvertisementStatus.Active, advertisement.Status);
        }

        [Fact]
        public void Advertisement_Should_Throw_When_Category_Is_Invalid()
        {
            // Arrange
            var category = Category.CreateNewCategory("Electronics", "products", "Eletrônicos", "Consumer electronics.");
            var address = UniLx.Domain.Entities.Seedwork.Address.CreateAddress(country: "BR", state: "RJ", city: "Rio de Janeiro", zipCode: "12345");
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
            var address = UniLx.Domain.Entities.Seedwork.Address.CreateAddress(country: "BR", state: "RJ", city: "Rio de Janeiro", zipCode: "12345");
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
            var category = Category.CreateNewCategory("real_estate", "Rentals", "Apartamentos Para Alugar", "Residential rental properties.");
            var invalidAddress = UniLx.Domain.Entities.Seedwork.Address.CreateAddress(country: "US", state: "NY", city: "New York", zipCode: "12345");
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
            var address = UniLx.Domain.Entities.Seedwork.Address.CreateAddress(country: "BR", state: "RJ", city: "Rio de Janeiro", zipCode: "12345");
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
        public void Advertisement_Status_Should_Be_Active_After_Initialization()
        {
            // Arrange
            var category = Category.CreateNewCategory("real_estate", "Rentals", "Apartamentos Para Alugar", "Residential rental properties.");
            var address = UniLx.Domain.Entities.Seedwork.Address.CreateAddress(country: "BR", state: "RJ", city: "Rio de Janeiro", zipCode: "12345");
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
            Assert.Equal(AdvertisementStatus.Active, advertisement.Status);
        }

        [Fact]
        public void Advertisement_IsExpired_Should_Return_True_For_Expired_Advertisement()
        {
            // Arrange
            var advertisement = new Advertisement(
                "real_estate",
                Category.CreateNewCategory("real_estate", "Rentals", "Apartamentos Para Alugar", "Residential rental properties."),
                new TestDetailsStub("Valid Title", "Valid Description", 100),
                DateTime.UtcNow.AddSeconds(1),
                UniLx.Domain.Entities.Seedwork.Address.CreateAddress(country: "BR", state: "RJ", city: "Rio de Janeiro", zipCode: "12345"),
                new Account("Owner", "owner@example.com", "15480563084", "Test owner", Guid.NewGuid().ToString())
            );

            Thread.Sleep(1001);

            // Act
            var isExpired = advertisement.IsExpired();

            // Assert
            Assert.True(isExpired);
        }

        [Fact]
        public void Advertisement_IsExpired_Should_Return_False_For_Non_Expired_Advertisement()
        {
            // Arrange
            var advertisement = new Advertisement(
                "real_estate",
                Category.CreateNewCategory("real_estate", "Rentals", "Apartamentos Para Alugar", "Residential rental properties."),
                new TestDetailsStub("Valid Title", "Valid Description", 100),
                DateTime.UtcNow.AddDays(10),
                UniLx.Domain.Entities.Seedwork.Address.CreateAddress(country: "BR", state: "RJ", city: "Rio de Janeiro", zipCode: "12345"),
                new Account("Owner", "owner@example.com", "15480563084", "Test owner", Guid.NewGuid().ToString())
            );

            // Act
            var isExpired = advertisement.IsExpired();

            // Assert
            Assert.False(isExpired);
        }

        [Fact]
        public void Advertisement_Rate_Should_Update_Ratings()
        {
            // Arrange
            var advertisementOwner = new Account("Owner", "owner@example.com", "24313678352", "Test owner", Guid.NewGuid().ToString());
            var account = new Account("User", "user@example.com", "24313678352", "Test user", Guid.NewGuid().ToString());
            var advertisement = new Advertisement(
                "real_estate",
                Category.CreateNewCategory("real_estate", "Rentals", "Apartamentos Para Alugar", "Residential rental properties."),
                new TestDetailsStub("Valid Title", "Valid Description", 100),
                DateTime.UtcNow.AddDays(10),
                UniLx.Domain.Entities.Seedwork.Address.CreateAddress(country: "BR", state: "RJ", city: "Rio de Janeiro", zipCode: "12345"),
                advertisementOwner
            );

            var chatRoom = new ChatRoom(account, advertisement);
            advertisement.Finish(advertisementOwner);

            // Act
            advertisement.Rate(4.5f, account, advertisementOwner);

            // Assert
            Assert.Equal(4.5f, advertisement.Rating.Value);
            Assert.Equal(4.5f, advertisementOwner.Rating.Value);
        }

        [Fact]
        public void Advertisement_Rate_Should_Throw_For_Invalid_Status()
        {
            // Arrange
            var advertisementOwner = new Account("Owner", "owner@example.com", "24313678352", "Test owner", Guid.NewGuid().ToString());
            var account = new Account("User", "user@example.com", "24313678352", "Test user", Guid.NewGuid().ToString());
            var advertisement = new Advertisement(
                "real_estate",
                Category.CreateNewCategory("real_estate", "Rentals", "Apartamentos Para Alugar", "Residential rental properties."),
                new TestDetailsStub("Valid Title", "Valid Description", 100),
                DateTime.UtcNow.AddDays(10),
                UniLx.Domain.Entities.Seedwork.Address.CreateAddress(country: "BR", state: "RJ", city: "Rio de Janeiro", zipCode: "12345"),
                advertisementOwner
            );

            var chatRoom = new ChatRoom(account, advertisement);

            // Act & Assert
            Assert.Throws<DomainException>(() => advertisement.Rate(4.5f, account, advertisementOwner));
        }

        [Fact]
        public void Advertisement_Finish_Should_Update_Status_To_Finished()
        {
            // Arrange
            var advertisementOwner = new Account("Owner", "owner@example.com", "15480563084", "Test owner", Guid.NewGuid().ToString());
            var advertisement = new Advertisement(
                "real_estate",
                Category.CreateNewCategory("real_estate", "Rentals", "Apartamentos Para Alugar", "Residential rental properties."),
                new TestDetailsStub("Valid Title", "Valid Description", 100),
                DateTime.UtcNow.AddDays(30),
                UniLx.Domain.Entities.Seedwork.Address.CreateAddress(country: "BR", state: "RJ", city: "Rio de Janeiro", zipCode: "12345"),
                advertisementOwner
            );

            // Act
            advertisement.Finish(advertisementOwner);

            // Assert
            Assert.Equal(AdvertisementStatus.Finished, advertisement.Status);
        }

        [Fact]
        public void Advertisement_Finish_Should_Throw_For_Invalid_Owner()
        {
            // Arrange
            var advertisementOwner = new Account("Owner", "owner@example.com", "15480563084", "Test owner", Guid.NewGuid().ToString());
            var anotherAccount = new Account("Other User", "other@example.com", "12345678909", "Other user", Guid.NewGuid().ToString());
            var advertisement = new Advertisement(
                "real_estate",
                Category.CreateNewCategory("real_estate", "Rentals", "Apartamentos Para Alugar", "Residential rental properties."),
                new TestDetailsStub("Valid Title", "Valid Description", 100),
                DateTime.UtcNow.AddDays(30),
                UniLx.Domain.Entities.Seedwork.Address.CreateAddress(country: "BR", state: "RJ", city: "Rio de Janeiro", zipCode: "12345"),
                advertisementOwner
            );

            // Act & Assert
            Assert.Throws<DomainException>(() => advertisement.Finish(anotherAccount));
        }
    }

    // Mock Details Implementation for Testing
    public class TestDetailsStub : Details
    {
        protected override AdvertisementType Type => AdvertisementType.RealEstate;

        public TestDetailsStub(string title, string description, int price) : base(title, description, price) { }
    }
}
