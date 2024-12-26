using System.Reflection;
using UniLx.Domain.Entities.AdvertisementAgg.Enumerations;
using UniLx.Domain.Entities.AdvertisementAgg.SpecificDetails;
using UniLx.Domain.Exceptions;

namespace UniLx.Tests.Domain.AdvertisementAgg
{
    public class RealEstateDetailsTests
    {
        [Fact]
        public void RealEstateDetails_Protected_Default_Constructor_Should_Work()
        {
            // Arrange
            var constructor = typeof(RealEstateDetails).GetConstructor(
                BindingFlags.NonPublic | BindingFlags.Instance,
                null,
                Type.EmptyTypes,
                null);

            // Act
            var details = (RealEstateDetails)constructor!.Invoke(null);

            // Assert
            Assert.NotNull(details);
            Assert.Null(details.Condition);
            Assert.Null(details.Bedrooms);
            Assert.Null(details.Bathrooms);
            Assert.Null(details.ParkingSpaces);
            Assert.Null(details.Floors);
            Assert.Null(details.AdditionalFeatures);
            Assert.Null(details.Price);
        }

        [Fact]
        public void RealEstateDetails_Should_Create_Valid_NonLand_Instance()
        {
            // Arrange
            var title = "Luxury Apartment";
            var description = "A spacious and modern apartment downtown.";
            var price = 500000;
            var lotSize = 120.5;
            var propertyType = "residential";
            var condition = "new";
            var constructedSqFt = 100.0;
            var bedrooms = 3;
            var bathrooms = 2;
            var parkingSpaces = 1;
            var floors = 1;
            var additionalFeatures = new List<string> { "Pool", "Gym" };

            // Act
            var details = new RealEstateDetails(
                title,
                description,
                price,
                lotSize,
                propertyType,
                condition,
                constructedSqFt,
                bedrooms,
                bathrooms,
                parkingSpaces,
                floors,
                additionalFeatures);

            // Assert
            Assert.NotNull(details);
            Assert.Equal(title, details.Title);
            Assert.Equal(description, details.Description);
            Assert.Equal(price, details.Price);
            Assert.Equal(lotSize, details.LotSizeInSquareMeters);
            Assert.Equal(PropertyType.Residential, details.PropertyType);
            Assert.Equal(PropertyCondition.New, details.Condition);
            Assert.Equal(constructedSqFt, details.ConstructedSquareFootage);
            Assert.Equal(bedrooms, details.Bedrooms);
            Assert.Equal(bathrooms, details.Bathrooms);
            Assert.Equal(parkingSpaces, details.ParkingSpaces);
            Assert.Equal(floors, details.Floors);
            Assert.Equal(additionalFeatures, details.AdditionalFeatures);
        }

        [Fact]
        public void RealEstateDetails_Should_Create_Valid_Land_Instance()
        {
            // Arrange
            var title = "Vacant Land";
            var description = "A large plot of land perfect for building.";
            var price = 200000;
            var lotSize = 500.0;
            var propertyType = "land";

            // Act
            var details = new RealEstateDetails(
                title,
                description,
                price,
                lotSize,
                propertyType,
                condition: null);

            // Assert
            Assert.NotNull(details);
            Assert.Equal(title, details.Title);
            Assert.Equal(description, details.Description);
            Assert.Equal(price, details.Price);
            Assert.Equal(lotSize, details.LotSizeInSquareMeters);
            Assert.Equal(PropertyType.Land, details.PropertyType);
            Assert.Null(details.Condition);
            Assert.Null(details.Bedrooms);
            Assert.Null(details.Bathrooms);
            Assert.Null(details.ParkingSpaces);
            Assert.Null(details.Floors);
            Assert.Null(details.AdditionalFeatures);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        public void RealEstateDetails_Should_Throw_When_Price_Is_Invalid(int? price)
        {
            // Arrange
            Action act = () => new RealEstateDetails("Title", "Description", price, 100, "residential", "new");

            // Act & Assert
            var ex = Assert.Throws<DomainException>(act);
            Assert.Contains("Price cannot be less or equal than 0", ex.Message);
        }

        [Fact]
        public void RealEstateDetails_Should_Throw_When_Price_Is_Null()
        {
            // Arrange
            Action act = () => new RealEstateDetails("Title", "Description", null, 100, "residential", "new");

            // Act & Assert
            var ex = Assert.Throws<DomainException>(act);
            Assert.Contains("Price is required for RealEstate details.", ex.Message);
        }

        [Theory]
        [InlineData(0.0)]
        [InlineData(-1.0)]
        public void RealEstateDetails_Should_Throw_When_LotSize_Is_Invalid(double lotSize)
        {
            // Arrange
            Action act = () => new RealEstateDetails("Title", "Description", 100000, lotSize, "residential", "new");

            // Act & Assert
            var ex = Assert.Throws<DomainException>(act);
            Assert.Contains("Lot size must be greater than zero", ex.Message);
        }

        [Theory]
        [InlineData("invalid_type")]
        [InlineData("invalidType")]
        [InlineData("invalidType2")]
        public void RealEstateDetails_Should_Throw_When_PropertyType_Is_Invalid(string propertyType)
        {
            // Arrange
            Action act = () => new RealEstateDetails("Title", "Description", 100000, 100, propertyType, "new");

            // Act & Assert
            var ex = Assert.Throws<DomainException>(act);
            Assert.Contains($"Invalid PropertyType. Allowed values are: {string.Join(", ", PropertyType.List.Select(p => p.Name))}.", ex.Message);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void RealEstateDetails_Should_Throw_When_PropertyType_Is_Null(string? propertyType)
        {
            // Arrange
            Action act = () => new RealEstateDetails("Title", "Description", 100000, 100, propertyType, "new");

            // Act & Assert
            var ex = Assert.Throws<DomainException>(act);
            Assert.Contains($"PropertyType cannot be null or empty.", ex.Message);
        }

        [Fact]
        public void RealEstateDetails_Should_Throw_When_Condition_Is_Required_But_Not_Provided()
        {
            // Arrange
            var propertyType = "commercial";
            Action act = () => new RealEstateDetails("Title", "Description", 100000, 200, propertyType, condition: null);

            // Act & Assert
            var ex = Assert.Throws<DomainException>(act);
            Assert.Contains("Condition is required for non-land properties", ex.Message);
        }

        [Fact]
        public void RealEstateDetails_Should_Throw_When_Condition_Is_Provided_For_Land()
        {
            // Arrange
            var propertyType = "land";
            var condition = "new";
            Action act = () => new RealEstateDetails("Title", "Description", 100000, 300, propertyType, condition);

            // Act & Assert
            var ex = Assert.Throws<DomainException>(act);
            Assert.Contains("Condition must not have a value for land", ex.Message);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(0)]
        public void RealEstateDetails_Should_Throw_When_Bedrooms_Is_Provided_For_Land(int bedrooms)
        {
            // Arrange
            var propertyType = "land";
            Action act = () => new RealEstateDetails("Title", "Description", 100000, 300, propertyType, null, bedrooms: bedrooms);

            // Act & Assert
            var ex = Assert.Throws<DomainException>(act);
            Assert.Contains("Bedrooms must not have a value for land", ex.Message);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(0)]
        public void RealEstateDetails_Should_Throw_When_Bathrooms_Is_Provided_For_Land(int bathrooms)
        {
            // Arrange
            var propertyType = "land";
            Action act = () => new RealEstateDetails("Title", "Description", 100000, 300, propertyType, null, bathrooms: bathrooms);

            // Act & Assert
            var ex = Assert.Throws<DomainException>(act);
            Assert.Contains("Bathrooms must not have a value for land", ex.Message);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(0)]
        public void RealEstateDetails_Should_Throw_When_ParkingSpaces_Is_Provided_For_Land(int parkingSpaces)
        {
            // Arrange
            var propertyType = "land";
            Action act = () => new RealEstateDetails("Title", "Description", 100000, 300, propertyType, null, parkingSpaces: parkingSpaces);

            // Act & Assert
            var ex = Assert.Throws<DomainException>(act);
            Assert.Contains("ParkingSpaces must not have a value for land", ex.Message);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(0)]
        public void RealEstateDetails_Should_Throw_When_Floors_Is_Provided_For_Land(int floors)
        {
            // Arrange
            var propertyType = "land";
            Action act = () => new RealEstateDetails("Title", "Description", 100000, 300, propertyType, null, floors: floors);

            // Act & Assert
            var ex = Assert.Throws<DomainException>(act);
            Assert.Contains("Floors must not have a value for land", ex.Message);
        }

        [Fact]
        public void RealEstateDetails_Should_Throw_When_AdditionalFeature_Is_Null_Or_Empty()
        {
            // Arrange
            var features = new List<string> { "Pool", "" };
            Action act = () => new RealEstateDetails("Title", "Description", 100000, 200, "residential", "new", additionalFeatures: features);

            // Act & Assert
            var ex = Assert.Throws<DomainException>(act);
            Assert.Contains("AdditionalFeatures cannot contain null or empty values", ex.Message);
        }

        [Fact]
        public void RealEstateDetails_Should_Throw_When_AdditionalFeature_Exceeds_Max_Length()
        {
            // Arrange
            var invalidFeature = new string('A', 51);
            var features = new List<string> { "Pool", invalidFeature };
            Action act = () => new RealEstateDetails("Title", "Description", 100000, 200, "residential", "new", additionalFeatures: features);

            // Act & Assert
            var ex = Assert.Throws<DomainException>(act);
            Assert.Contains("Each additional feature must be 50 characters or less", ex.Message);
        }

        [Theory]
        [InlineData(-10.0)]
        [InlineData(0.0)]
        public void RealEstateDetails_Should_Throw_When_ConstructedSquareFootage_Is_Invalid(double constructedSqFt)
        {
            // Arrange
            Action act = () => new RealEstateDetails("Title", "Description", 100000, 200, "residential", "new", constructedSquareFootage: constructedSqFt);

            // Act & Assert
            var ex = Assert.Throws<DomainException>(act);
            Assert.Contains("Constructed square footage, if provided, must be greater than zero", ex.Message);
        }

        [Fact]
        public void RealEstateDetails_Should_Allow_Null_ConstructedSquareFootage()
        {
            // Arrange

            // Act
            var details = new RealEstateDetails("Title", "Description", 100000, 200, "residential", "new");

            // Assert
            Assert.Null(details.ConstructedSquareFootage);
        }
    }
}
