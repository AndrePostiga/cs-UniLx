using System.Reflection;
using UniLx.Domain.Entities.AdvertisementAgg.Enumerations;
using UniLx.Domain.Entities.AdvertisementAgg.SpecificDetails;
using UniLx.Domain.Exceptions;

namespace UniLx.Tests.Domain.AdvertisementAgg
{
    public class ElectronicsDetailsTests
    {
        [Theory]
        [InlineData("new")]
        [InlineData("like_new")]
        [InlineData("used")]
        [InlineData("refurbished")]
        public void ElectronicsDetails_Should_Create_Valid_Instance(string conditionName)
        {
            // Arrange
            var title = "Gaming Laptop";
            var description = "A high-performance gaming laptop.";
            var price = 15000;
            var productType = "Laptop";
            var brand = "Dell";
            var model = "Alienware M15";
            var storageCapacity = "1TB";
            var memory = "16GB";
            var processor = "Intel i9";
            var graphicsCard = "NVIDIA RTX 3080";
            var batteryLife = 0.8f;
            var warrantyUntil = DateTime.UtcNow.AddYears(1);
            var features = new List<string> { "4K Display", "Bluetooth 5.0" };
            var includesOriginalBox = true;
            var accessories = new List<string> { "Charger", "Mouse" };

            // Act
            var details = new ElectronicsDetails(
                title, description, price, productType, brand, model, storageCapacity,
                memory, processor, graphicsCard, batteryLife, warrantyUntil, features,
                conditionName, includesOriginalBox, accessories);

            // Assert
            Assert.NotNull(details);
            Assert.Equal(title, details.Title);
            Assert.Equal(description, details.Description);
            Assert.Equal(price, details.Price);
            Assert.Equal(productType, details.ProductType);
            Assert.Equal(brand, details.Brand);
            Assert.Equal(model, details.Model);
            Assert.Equal(storageCapacity, details.StorageCapacity);
            Assert.Equal(memory, details.Memory);
            Assert.Equal(processor, details.Processor);
            Assert.Equal(graphicsCard, details.GraphicsCard);
            Assert.Equal(batteryLife, details.BatteryLife);
            Assert.Equal(warrantyUntil, details.WarrantyUntil);
            Assert.Equal(features, details.Features);
            Assert.Equal(accessories, details.Accessories);

            // Check condition
            var expectedCondition = ProductCondition.FromName(conditionName, ignoreCase: true);
            Assert.Equal(expectedCondition.Name, details.Condition.Name);

            Assert.True(details.HasWarranty);
            Assert.True(details.IncludesAccessories);
            Assert.True(details.IncludesOriginalBox);
        }

        [Fact]
        public void ElectronicsDetails_Should_Throw_When_ProductType_Is_Null_Or_Empty()
        {
            // Arrange
            string? invalidProductType = null;

            // Act & Assert
            Assert.Throws<DomainException>(() =>
                new ElectronicsDetails("Title", "Description", 1000, invalidProductType, "Brand", "Model", "1TB", "16GB", "Processor", "GraphicsCard", null, null, null, "New", true, null));
        }

        [Fact]
        public void ElectronicsDetails_Should_Throw_When_ProductType_Exceeds_Max_Length()
        {
            // Arrange
            var invalidProductType = new string('A', 101);

            // Act & Assert
            Assert.Throws<DomainException>(() =>
                new ElectronicsDetails("Title", "Description", 1000, invalidProductType, "Brand", "Model", "1TB", "16GB", "Processor", "GraphicsCard", null, null, null, "New", true, null));
        }

        [Fact]
        public void ElectronicsDetails_Should_Throw_When_StorageCapacity_Is_Invalid()
        {
            // Arrange
            var invalidStorageCapacity = "100MBs";

            // Act & Assert
            Assert.Throws<DomainException>(() =>
                new ElectronicsDetails("Title", "Description", 1000, "Laptop", "Brand", "Model", invalidStorageCapacity, "16GB", "Processor", "GraphicsCard", null, null, null, "New", true, null));
        }

        [Fact]
        public void ElectronicsDetails_Should_Throw_When_Memory_Is_Invalid()
        {
            // Arrange
            var invalidMemory = "16 Megabytes";

            // Act & Assert
            Assert.Throws<DomainException>(() =>
                new ElectronicsDetails("Title", "Description", 1000, "Laptop", "Brand", "Model", "1TB", invalidMemory, "Processor", "GraphicsCard", null, null, null, "New", true, null));
        }

        [Fact]
        public void ElectronicsDetails_Should_Throw_When_BatteryLife_Is_Out_Of_Range()
        {
            // Arrange
            var invalidBatteryLife = 1.5f;

            // Act & Assert
            Assert.Throws<DomainException>(() =>
                new ElectronicsDetails("Title", "Description", 1000, "Laptop", "Brand", "Model", "1TB", "16GB", "Processor", "GraphicsCard", invalidBatteryLife, null, null, "New", true, null));
        }

        [Fact]
        public void ElectronicsDetails_Should_Throw_When_WarrantyUntil_Is_In_The_Past()
        {
            // Arrange
            var pastWarranty = DateTime.UtcNow.AddMonths(-1);

            // Act & Assert
            Assert.Throws<DomainException>(() =>
                new ElectronicsDetails("Title", "Description", 1000, "Laptop", "Brand", "Model", "1TB", "16GB", "Processor", "GraphicsCard", null, pastWarranty, null, "New", true, null));
        }

        [Fact]
        public void ElectronicsDetails_Should_Allow_Null_WarrantyUntil()
        {
            // Act
            var details = new ElectronicsDetails("Title", "Description", 1000, "Laptop", "Brand", "Model", "1TB", "16GB", "Processor", "GraphicsCard", null, null, null, "New", true, null);

            // Assert
            Assert.Null(details.WarrantyUntil);
            Assert.False(details.HasWarranty);
        }

        [Fact]
        public void ElectronicsDetails_Should_Throw_When_Feature_Exceeds_Max_Length()
        {
            // Arrange
            var invalidFeatures = new List<string> { "4K Display", new string('A', 51) };

            // Act & Assert
            Assert.Throws<DomainException>(() =>
                new ElectronicsDetails("Title", "Description", 1000, "Laptop", "Brand", "Model", "1TB", "16GB", "Processor", "GraphicsCard", null, null, invalidFeatures, "New", true, null));
        }

        [Fact]
        public void ElectronicsDetails_Should_Throw_When_Accessory_Exceeds_Max_Length()
        {
            // Arrange
            var invalidAccessories = new List<string> { "Charger", new string('A', 51) };

            // Act & Assert
            Assert.Throws<DomainException>(() =>
                new ElectronicsDetails("Title", "Description", 1000, "Laptop", "Brand", "Model", "1TB", "16GB", "Processor", "GraphicsCard", null, null, null, "New", true, invalidAccessories));
        }

        [Fact]
        public void ElectronicsDetails_Protected_Default_Constructor_Should_Work()
        {
            // Act
            var constructor = typeof(ElectronicsDetails).GetConstructor(
                BindingFlags.NonPublic | BindingFlags.Instance,
                null,
                Type.EmptyTypes,
                null);

            var details = (ElectronicsDetails)constructor!.Invoke(null);

            // Assert
            Assert.NotNull(details);
            Assert.Null(details.ProductType);
            Assert.Null(details.Brand);
            Assert.Null(details.Model);
            Assert.Null(details.StorageCapacity);
            Assert.Null(details.Memory);
            Assert.Null(details.Processor);
            Assert.Null(details.GraphicsCard);
            Assert.Null(details.BatteryLife);
            Assert.Null(details.WarrantyUntil);
            Assert.Null(details.Features);
            Assert.Null(details.Accessories);
            Assert.False(details.HasWarranty);
        }
    }

}
