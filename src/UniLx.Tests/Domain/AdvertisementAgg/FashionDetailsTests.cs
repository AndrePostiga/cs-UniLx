using System.Reflection;
using UniLx.Domain.Entities.AdvertisementAgg.SpecificDetails;
using UniLx.Domain.Entities.AdvertisementAgg.Enumerations;
using UniLx.Domain.Exceptions;

namespace UniLx.Tests.Domain.AdvertisementAgg
{
    public class FashionDetailsTests
    {
        [Theory]
        [InlineData("pp", "male")]
        [InlineData("p", "female")]
        [InlineData("m", "unisex")]
        [InlineData("g", "male")]
        [InlineData("gg", "female")]
        [InlineData("xg", "unisex")]
        [InlineData("xxg", "male")]
        [InlineData("xxxg", "female")]
        [InlineData("plus_size", "unisex")]
        public void FashionDetails_Should_Create_Valid_Instance(string size, string gender)
        {
            // Arrange
            var title = "Elegant Dress";
            var description = "A stylish and comfortable dress.";
            var price = 200;
            var clothingType = "Dress";
            var brand = "Gucci";
            var sizes = new List<string> { size };
            var colors = new List<string> { "Red", "Blue" };
            var materials = new List<string> { "Cotton", "Polyester" };
            var features = new List<string> { "Breathable", "Waterproof" };
            var designer = "Famous Designer";
            var isHandmade = true;
            var releaseDate = DateTime.UtcNow.AddMonths(-3);
            var isSustainable = true;

            // Act
            var details = new FashionDetails(
                title, description, price, clothingType, brand, sizes, gender, colors, materials, features, designer, isHandmade, releaseDate, isSustainable);

            // Assert
            Assert.NotNull(details);
            Assert.Equal(title, details.Title);
            Assert.Equal(description, details.Description);
            Assert.Equal(price, details.Price);
            Assert.Equal(clothingType, details.ClothingType);
            Assert.Equal(brand, details.Brand);
            Assert.Equal(FashionSize.FromName(size, ignoreCase: true), details.Sizes.Single());
            Assert.Equal(FashionGender.FromName(gender, ignoreCase: true), details.Gender);
            Assert.Equal(colors, details.Colors);
            Assert.Equal(materials, details.Materials);
            Assert.Equal(features, details.Features);
            Assert.Equal(designer, details.Designer);
            Assert.Equal(isHandmade, details.IsHandmade);
            Assert.Equal(releaseDate, details.ReleaseDate);
            Assert.Equal(isSustainable, details.IsSustainable);
        }

        [Fact]
        public void FashionDetails_Should_Throw_When_ClothingType_Is_Null_Or_Empty()
        {
            // Arrange
            string? invalidClothingType = null;

            // Act & Assert
            Assert.Throws<DomainException>(() =>
                new FashionDetails("Title", "Description", 200, invalidClothingType, "Brand", new List<string> { "M" }, "male"));
        }

        [Fact]
        public void FashionDetails_Should_Throw_When_Size_Is_Invalid()
        {
            // Arrange
            var invalidSizes = new List<string> { "InvalidSize" };

            // Act & Assert
            Assert.Throws<DomainException>(() =>
                new FashionDetails("Title", "Description", 200, "T-shirt", "Brand", invalidSizes, "male"));
        }

        [Fact]
        public void FashionDetails_Should_Throw_When_Gender_Is_Invalid()
        {
            // Arrange
            var invalidGender = "invalidGender";

            // Act & Assert
            Assert.Throws<DomainException>(() =>
                new FashionDetails("Title", "Description", 200, "T-shirt", "Brand", new List<string> { "M" }, invalidGender));
        }

        [Fact]
        public void FashionDetails_Should_Throw_When_Color_Exceeds_Max_Length()
        {
            // Arrange
            var invalidColors = new List<string> { "ValidColor", new string('A', 51) };

            // Act & Assert
            Assert.Throws<DomainException>(() =>
                new FashionDetails("Title", "Description", 200, "T-shirt", "Brand", new List<string> { "M" }, "male", invalidColors));
        }

        [Fact]
        public void FashionDetails_Should_Throw_When_Material_Exceeds_Max_Length()
        {
            // Arrange
            var invalidMaterials = new List<string> { "Cotton", new string('A', 51) };

            // Act & Assert
            Assert.Throws<DomainException>(() =>
                new FashionDetails("Title", "Description", 200, "T-shirt", "Brand", new List<string> { "M" }, "male", null, invalidMaterials));
        }

        [Fact]
        public void FashionDetails_Should_Throw_When_Feature_Exceeds_Max_Length()
        {
            // Arrange
            var invalidFeatures = new List<string> { "Breathable", new string('A', 51) };

            // Act & Assert
            Assert.Throws<DomainException>(() =>
                new FashionDetails("Title", "Description", 200, "T-shirt", "Brand", new List<string> { "M" }, "male", null, null, invalidFeatures));
        }

        [Fact]
        public void FashionDetails_Protected_Default_Constructor_Should_Work()
        {
            // Act
            var constructor = typeof(FashionDetails).GetConstructor(
                BindingFlags.NonPublic | BindingFlags.Instance,
                null,
                Type.EmptyTypes,
                null);

            var details = (FashionDetails)constructor.Invoke(null);

            // Assert
            Assert.NotNull(details);
            Assert.Null(details.ClothingType);
            Assert.Null(details.Brand);
            Assert.Null(details.Sizes);
            Assert.Null(details.Colors);
            Assert.Null(details.Materials);
            Assert.Null(details.Features);
            Assert.Null(details.Designer);
            Assert.Null(details.IsHandmade);
            Assert.Null(details.ReleaseDate);
            Assert.Null(details.IsSustainable);
        }
    }
}
