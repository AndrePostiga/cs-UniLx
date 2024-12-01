using Ardalis.SmartEnum;
using UniLx.Domain.Entities.AdvertisementAgg.Enumerations;

namespace UniLx.Tests.Domain.AdvertisementAgg.Enumerations
{
    public class ProductConditionTests
    {
        [Theory]
        [InlineData("new", 1)]
        [InlineData("like_new", 2)]
        [InlineData("used", 3)]
        [InlineData("refurbished", 4)]
        public void ProductCondition_ShouldHaveCorrectNameAndValue(string expectedName, int expectedValue)
        {
            // Act
            var productCondition = ProductCondition.List.FirstOrDefault(pc => pc.Value == expectedValue && pc.Name == expectedName);

            // Assert
            Assert.NotNull(productCondition);
            Assert.Equal(expectedName, productCondition!.Name);
            Assert.Equal(expectedValue, productCondition!.Value);
        }

        [Theory]
        [InlineData("new", true)]
        [InlineData("like_new", true)]
        [InlineData("used", true)]
        [InlineData("refurbished", true)]
        [InlineData("unknown", false)]
        public void ProductCondition_TryFromName_ShouldReturnCorrectResult(string name, bool expectedResult)
        {
            // Act
            var result = ProductCondition.TryFromName(name, ignoreCase: true, out var productCondition);

            // Assert
            Assert.Equal(expectedResult, result);
            if (expectedResult)
            {
                Assert.NotNull(productCondition);
                Assert.Equal(name, productCondition!.Name);
            }
            else
            {
                Assert.Null(productCondition);
            }
        }

        [Fact]
        public void ProductCondition_List_ShouldContainAllTypes()
        {
            // Arrange
            var expectedCount = 4;

            // Act
            var allConditions = ProductCondition.List;

            // Assert
            Assert.Equal(expectedCount, allConditions.Count);
            Assert.Contains(ProductCondition.New, allConditions);
            Assert.Contains(ProductCondition.LikeNew, allConditions);
            Assert.Contains(ProductCondition.Used, allConditions);
            Assert.Contains(ProductCondition.Refurbished, allConditions);
        }

        [Theory]
        [InlineData(1, "new")]
        [InlineData(2, "like_new")]
        [InlineData(3, "used")]
        [InlineData(4, "refurbished")]
        public void ProductCondition_FromValue_ShouldReturnCorrectType(int value, string expectedName)
        {
            // Act
            var productCondition = ProductCondition.FromValue(value);

            // Assert
            Assert.NotNull(productCondition);
            Assert.Equal(expectedName, productCondition.Name);
            Assert.Equal(value, productCondition.Value);
        }

        [Fact]
        public void ProductCondition_InvalidFromValue_ShouldThrowException()
        {
            // Act & Assert
            Assert.Throws<SmartEnumNotFoundException>(() => ProductCondition.FromValue(99));
        }
    }

}
