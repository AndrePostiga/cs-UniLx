using System.Reflection;
using UniLx.Domain.Entities.AdvertisementAgg.SpecificDetails;
using UniLx.Domain.Exceptions;

namespace UniLx.Tests.Domain.AdvertisementAgg
{
    public class BeautyDetailsTests
    {
        [Fact]
        public void BeautyDetails_Protected_Default_Constructor_Should_Work()
        {
            // Act
            var constructor = typeof(BeautyDetails).GetConstructor(
                BindingFlags.NonPublic | BindingFlags.Instance,
                null,
                Type.EmptyTypes,
                null);

            var beautyDetails = (BeautyDetails)Activator.CreateInstance(typeof(BeautyDetails), true)!;

            // Assert
            Assert.NotNull(beautyDetails);
            Assert.Null(beautyDetails.ProductType);
            Assert.Null(beautyDetails.Brand);
            Assert.Null(beautyDetails.SkinType);
            Assert.Null(beautyDetails.ExpirationDate);
            Assert.Null(beautyDetails.Ingredients);
            Assert.Null(beautyDetails.IsOrganic);
        }

        [Fact]
        public void BeautyDetails_Should_Create_Valid_Instance()
        {
            // Arrange
            var title = "Skincare Product";
            var description = "A high-quality skincare product.";
            var price = 100;
            var productType = "Skincare";
            var brand = "L'Oreal";
            var skinType = "Dry";
            var expirationDate = DateTime.UtcNow.AddDays(30);
            var ingredients = new List<string> { "Water", "Glycerin" };
            var isOrganic = true;

            // Act
            var details = new BeautyDetails(title, description, price, productType, brand, skinType, expirationDate, ingredients, isOrganic);

            // Assert
            Assert.NotNull(details);
            Assert.Equal(title, details.Title);
            Assert.Equal(description, details.Description);
            Assert.Equal(price, details.Price);
            Assert.Equal(productType, details.ProductType);
            Assert.Equal(brand, details.Brand);
            Assert.Equal(skinType, details.SkinType);
            Assert.Equal(expirationDate, details.ExpirationDate);
            Assert.Equal(ingredients, details.Ingredients);
            Assert.Equal(isOrganic, details.IsOrganic);
        }

        [Fact]
        public void BeautyDetails_Should_Throw_When_ProductType_Is_Null_Or_Empty()
        {
            // Arrange
            string? invalidProductType = null;

            // Act & Assert
            Assert.Throws<DomainException>(() =>
            {
                new BeautyDetails("Title", "Description", 100, invalidProductType, "Brand", "SkinType", null, null, null);
            });
        }

        [Fact]
        public void BeautyDetails_Should_Throw_When_ProductType_Exceeds_Max_Length()
        {
            // Arrange
            var invalidProductType = new string('A', 101);

            // Act & Assert
            Assert.Throws<DomainException>(() =>
            {
                new BeautyDetails("Title", "Description", 100, invalidProductType, "Brand", "SkinType", null, null, null);
            });
        }

        [Fact]
        public void BeautyDetails_Should_Throw_When_Brand_Is_Null_Or_Empty()
        {
            // Arrange
            string? invalidBrand = null;

            // Act & Assert
            Assert.Throws<DomainException>(() =>
            {
                new BeautyDetails("Title", "Description", 100, "ProductType", invalidBrand, "SkinType", null, null, null);
            });
        }

        [Fact]
        public void BeautyDetails_Should_Throw_When_Brand_Exceeds_Max_Length()
        {
            // Arrange
            var invalidBrand = new string('A', 101);

            // Act & Assert
            Assert.Throws<DomainException>(() =>
            {
                new BeautyDetails("Title", "Description", 100, "ProductType", invalidBrand, "SkinType", null, null, null);
            });
        }

        [Fact]
        public void BeautyDetails_Should_Throw_When_SkinType_Is_Null_Or_Empty()
        {
            // Arrange
            string? invalidSkinType = null;

            // Act & Assert
            Assert.Throws<DomainException>(() =>
            {
                new BeautyDetails("Title", "Description", 100, "ProductType", "Brand", invalidSkinType, null, null, null);
            });
        }

        [Fact]
        public void BeautyDetails_Should_Throw_When_SkinType_Exceeds_Max_Length()
        {
            // Arrange
            var invalidSkinType = new string('A', 51);

            // Act & Assert
            Assert.Throws<DomainException>(() =>
            {
                new BeautyDetails("Title", "Description", 100, "ProductType", "Brand", invalidSkinType, null, null, null);
            });
        }

        [Fact]
        public void BeautyDetails_Should_Throw_When_ExpirationDate_Is_In_The_Past()
        {
            // Arrange
            var pastExpirationDate = DateTime.UtcNow.AddDays(-1);

            // Act & Assert
            Assert.Throws<DomainException>(() =>
            {
                new BeautyDetails("Title", "Description", 100, "ProductType", "Brand", "SkinType", pastExpirationDate, null, null);
            });
        }

        [Fact]
        public void BeautyDetails_Should_Allow_Null_ExpirationDate()
        {
            // Act
            var details = new BeautyDetails("Title", "Description", 100, "ProductType", "Brand", "SkinType", null, null, null);

            // Assert
            Assert.Null(details.ExpirationDate);
        }

        [Fact]
        public void BeautyDetails_Should_Throw_When_Ingredients_List_Is_Empty()
        {
            // Arrange
            var emptyIngredients = new List<string>();

            // Act & Assert
            Assert.Throws<DomainException>(() =>
            {
                new BeautyDetails("Title", "Description", 100, "ProductType", "Brand", "SkinType", null, emptyIngredients, null);
            });
        }

        [Fact]
        public void BeautyDetails_Should_Throw_When_Ingredients_Contain_Null_Or_Empty_Entries()
        {
            // Arrange
            var invalidIngredients = new List<string> { "Water", "" };

            // Act & Assert
            Assert.Throws<DomainException>(() =>
            {
                new BeautyDetails("Title", "Description", 100, "ProductType", "Brand", "SkinType", null, invalidIngredients, null);
            });
        }

        [Fact]
        public void BeautyDetails_Should_Throw_When_Ingredient_Exceeds_Max_Length()
        {
            // Arrange
            var invalidIngredients = new List<string> { "Water", new string('A', 51) };

            // Act & Assert
            Assert.Throws<DomainException>(() =>
            {
                new BeautyDetails("Title", "Description", 100, "ProductType", "Brand", "SkinType", null, invalidIngredients, null);
            });
        }

        [Fact]
        public void BeautyDetails_Should_Allow_Null_Ingredients()
        {
            // Act
            var details = new BeautyDetails("Title", "Description", 100, "ProductType", "Brand", "SkinType", null, null, null);

            // Assert
            Assert.Null(details.Ingredients);
        }

        [Fact]
        public void BeautyDetails_Should_Set_IsOrganic_Properly()
        {
            // Act
            var details = new BeautyDetails("Title", "Description", 100, "ProductType", "Brand", "SkinType", null, null, true);

            // Assert
            Assert.True(details.IsOrganic);
        }
    }

}
