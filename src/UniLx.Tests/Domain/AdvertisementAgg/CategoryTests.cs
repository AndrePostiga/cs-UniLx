using UniLx.Domain.Entities.AdvertisementAgg;
using UniLx.Domain.Exceptions;

namespace UniLx.Tests.Domain.AdvertisementAgg
{
    public class CategoryTests
    {
        [Fact]
        public void Category_Should_Create_Valid_Category()
        {
            // Arrange
            var root = "Fashion";
            var name = "Clothing";
            var nameInPtBr = "Roupas";
            var description = "Category for clothing advertisements";

            // Act
            var category = Category.CreateNewCategory(root, name, nameInPtBr, description);

            // Assert
            Assert.NotNull(category);
            Assert.Equal(root.ToLower(), category.Root.Name);
            Assert.Equal(name.ToLower(), category.Name);
            Assert.Equal(nameInPtBr, category.NameInPtBr);
            Assert.Equal(description, category.Description);
            Assert.True(category.IsActive);
        }

        [Fact]
        public void Category_Should_Throw_On_Invalid_Root_Type()
        {
            // Arrange
            var invalidRoot = "InvalidRoot";
            var name = "Clothing";
            var nameInPtBr = "Roupas";
            var description = "Category for clothing advertisements";

            // Act & Assert
            Assert.Throws<DomainException>(() =>
            {
                Category.CreateNewCategory(invalidRoot, name, nameInPtBr, description);
            });
        }

        [Fact]
        public void Category_Should_Throw_On_Invalid_Name_Same_As_AdvertisementType()
        {
            // Arrange
            var root = "Fashion";
            var invalidName = "Fashion"; // Same as root
            var nameInPtBr = "Roupas";
            var description = "Invalid category name matching advertisement type";

            // Act & Assert
            Assert.Throws<DomainException>(() =>
            {
                Category.CreateNewCategory(root, invalidName, nameInPtBr, description);
            });
        }

        [Fact]
        public void Category_Should_Throw_On_Name_Exceeding_Max_Length()
        {
            // Arrange
            var root = "Fashion";
            var name = new string('A', 101); // Exceeds max length of 100
            var nameInPtBr = "Roupas";
            var description = "Category with excessively long name";

            // Act & Assert
            Assert.Throws<DomainException>(() =>
            {
                Category.CreateNewCategory(root, name, nameInPtBr, description);
            });
        }

        [Fact]
        public void Category_Should_Throw_On_Description_Exceeding_Max_Length()
        {
            // Arrange
            var root = "Fashion";
            var name = "Clothing";
            var nameInPtBr = "Roupas";
            var description = new string('A', 257); // Exceeds max length of 256

            // Act & Assert
            Assert.Throws<DomainException>(() =>
            {
                Category.CreateNewCategory(root, name, nameInPtBr, description);
            });
        }
    }
}
