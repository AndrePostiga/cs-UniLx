using Ardalis.SmartEnum;
using UniLx.Domain.Entities.AdvertisementAgg.Enumerations;

namespace UniLx.Tests.Domain.AdvertisementAgg.Enumerations
{
    public class PropertyTypeTests
    {
        [Theory]
        [InlineData("residential", 1)]
        [InlineData("commercial", 2)]
        [InlineData("land", 3)]
        public void PropertyType_ShouldHaveCorrectNameAndValue(string expectedName, int expectedValue)
        {
            // Act
            var propertyType = PropertyType.List.FirstOrDefault(pt => pt.Value == expectedValue);

            // Assert
            Assert.NotNull(propertyType);
            Assert.Equal(expectedName, propertyType!.Name);
            Assert.Equal(expectedValue, propertyType!.Value);
        }

        [Theory]
        [InlineData("residential", true)]
        [InlineData("commercial", true)]
        [InlineData("land", true)]
        [InlineData("invalid_type", false)]
        public void PropertyType_TryFromName_ShouldReturnCorrectResult(string name, bool expectedResult)
        {
            // Act
            var result = PropertyType.TryFromName(name, ignoreCase: true, out var propertyType);

            // Assert
            Assert.Equal(expectedResult, result);
            if (expectedResult)
            {
                Assert.NotNull(propertyType);
                Assert.Equal(name, propertyType!.Name);
            }
            else
            {
                Assert.Null(propertyType);
            }
        }

        [Fact]
        public void PropertyType_List_ShouldContainAllTypes()
        {
            // Arrange
            var expectedCount = 3;

            // Act
            var allPropertyTypes = PropertyType.List;

            // Assert
            Assert.Equal(expectedCount, allPropertyTypes.Count);
            Assert.Contains(PropertyType.Residential, allPropertyTypes);
            Assert.Contains(PropertyType.Commercial, allPropertyTypes);
            Assert.Contains(PropertyType.Land, allPropertyTypes);
        }

        [Theory]
        [InlineData(1, "residential")]
        [InlineData(3, "land")]
        public void PropertyType_FromValue_ShouldReturnCorrectType(int value, string expectedName)
        {
            // Act
            var propertyType = PropertyType.FromValue(value);

            // Assert
            Assert.NotNull(propertyType);
            Assert.Equal(expectedName, propertyType.Name);
            Assert.Equal(value, propertyType.Value);
        }

        [Fact]
        public void PropertyType_InvalidFromValue_ShouldThrowException()
        {
            // Act & Assert
            Assert.Throws<SmartEnumNotFoundException>(() => PropertyType.FromValue(99));
        }
    }
}
