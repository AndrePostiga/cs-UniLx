using System.Reflection;
using UniLx.Domain.Entities.AdvertisementAgg.Enumerations;
using UniLx.Domain.Entities.AdvertisementAgg.SpecificDetails;
using UniLx.Domain.Exceptions;
using Xunit;

namespace UniLx.Tests.Domain.AdvertisementAgg
{
    public class OthersDetailsTests
    {
        [Fact]
        public void OthersDetails_Protected_Default_Constructor_Should_Work()
        {
            // Arrange
            var constructor = typeof(OthersDetails).GetConstructor(
                BindingFlags.NonPublic | BindingFlags.Instance,
                null,
                Type.EmptyTypes,
                null);

            // Act
            var details = (OthersDetails)constructor!.Invoke(null);

            // Assert
            Assert.NotNull(details);
            Assert.Null(details.Condition);
            Assert.Null(details.Brand);
            Assert.Null(details.Features);
            Assert.Null(details.WarrantyUntil);
            Assert.Null(details.Price);
        }

        [Theory]
        [InlineData("new")]
        [InlineData("like_new")]
        [InlineData("used")]
        [InlineData("refurbished")]
        public void OthersDetails_Should_Create_Valid_Instance_With_Condition(string conditionName)
        {
            // Arrange
            var title = "Generic Item";
            var description = "A useful generic item.";
            var price = 1000;
            var brand = "BrandName";
            var features = new List<string> { "Durable", "Lightweight" };
            var warrantyUntil = DateTime.UtcNow.AddDays(30);

            // Act
            var details = new OthersDetails(
                title,
                description,
                price,
                conditionName,
                brand,
                features,
                warrantyUntil
            );

            // Assert
            Assert.NotNull(details);
            Assert.Equal(title, details.Title);
            Assert.Equal(description, details.Description);
            Assert.Equal(price, details.Price);
            Assert.Equal(ProductCondition.FromName(conditionName, true), details.Condition);
            Assert.Equal(brand, details.Brand);
            Assert.Equal(features, details.Features);
            Assert.Equal(warrantyUntil, details.WarrantyUntil);
        }

        [Fact]
        public void OthersDetails_Should_Create_Valid_Instance_Without_Condition_Or_Other_Optional_Fields()
        {
            // Arrange
            var title = "Generic Item";
            var description = "A useful generic item.";
            var price = 150;

            // Act
            var details = new OthersDetails(title, description, price);

            // Assert
            Assert.NotNull(details);
            Assert.Equal(title, details.Title);
            Assert.Equal(description, details.Description);
            Assert.Equal(price, details.Price);
            Assert.Null(details.Condition);
            Assert.Null(details.Brand);
            Assert.Null(details.Features);
            Assert.Null(details.WarrantyUntil);
        }

        [Fact]
        public void OthersDetails_Should_Throw_When_Condition_Is_Invalid()
        {
            // Arrange
            var invalidCondition = "invalid_condition";
            Action act = () => new OthersDetails("Title", "Desc", 100, invalidCondition);

            // Act & Assert
            var ex = Assert.Throws<DomainException>(act);
            Assert.Contains("Invalid product condition", ex.Message);
        }

        [Fact]
        public void OthersDetails_Should_Throw_When_Brand_Exceeds_Max_Length()
        {
            // Arrange
            var invalidBrand = new string('B', 101);
            Action act = () => new OthersDetails("Title", "Desc", 100, "new", invalidBrand);

            // Act & Assert
            var ex = Assert.Throws<DomainException>(act);
            Assert.Contains("Brand must be 100 characters or less", ex.Message);
        }

        [Fact]
        public void OthersDetails_Should_Throw_When_Features_Contain_Null_Or_Empty_Values()
        {
            // Arrange
            var invalidFeatures = new List<string> { "Durable", "" };
            Action act = () => new OthersDetails("Title", "Desc", 100, "new", "Brand", invalidFeatures);

            // Act & Assert
            var ex = Assert.Throws<DomainException>(act);
            Assert.Contains("Features cannot contain null or empty values", ex.Message);
        }

        [Fact]
        public void OthersDetails_Should_Throw_When_Features_Exceed_Max_Length()
        {
            // Arrange
            var invalidFeature = new string('F', 51);
            var features = new List<string> { "Durable", invalidFeature };
            Action act = () => new OthersDetails("Title", "Desc", 100, "new", "Brand", features);

            // Act & Assert
            var ex = Assert.Throws<DomainException>(act);
            Assert.Contains("Each feature must be 50 characters or less", ex.Message);
        }

        [Fact]
        public void OthersDetails_Should_Throw_When_WarrantyUntil_Is_In_The_Past()
        {
            // Arrange
            var pastDate = DateTime.UtcNow.AddDays(-1);
            Action act = () => new OthersDetails("Title", "Desc", 100, "new", "Brand", null, pastDate);

            // Act & Assert
            var ex = Assert.Throws<DomainException>(act);
            Assert.Contains("WarrantyUntil must be in the future", ex.Message);
        }

        [Fact]
        public void OthersDetails_Should_Allow_Null_WarrantyUntil()
        {
            // Arrange & Act
            var details = new OthersDetails("Title", "Desc", 100, "new");

            // Assert
            Assert.NotNull(details);
            Assert.Null(details.WarrantyUntil);
        }
    }
}
