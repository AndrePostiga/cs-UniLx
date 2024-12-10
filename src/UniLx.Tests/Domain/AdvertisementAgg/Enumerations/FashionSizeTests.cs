using Ardalis.SmartEnum;
using UniLx.Domain.Entities.AdvertisementAgg.Enumerations;

namespace UniLx.Tests.Domain.AdvertisementAgg.Enumerations
{
    public class FashionSizeTests
    {
        [Theory]
        [InlineData("pp", 1)] // Extra Small
        [InlineData("p", 2)]  // Small
        [InlineData("m", 3)]  // Medium
        [InlineData("g", 4)]  // Large
        [InlineData("gg", 5)] // Extra Large
        [InlineData("xg", 6)] // Extra Extra Large
        [InlineData("xxg", 6)] // Extra Extra Large
        [InlineData("xxxg", 6)] // Extra Extra Large
        [InlineData("plus_size", 6)] // Plus Size
        public void FashionSize_ShouldHaveCorrectNameAndValue(string expectedName, int expectedValue)
        {
            // Act
            var fashionSize = FashionSize.List.FirstOrDefault(fs => fs.Value == expectedValue && fs.Name == expectedName);

            // Assert
            Assert.NotNull(fashionSize);
            Assert.Equal(expectedName, fashionSize!.Name);
            Assert.Equal(expectedValue, fashionSize!.Value);
        }

        [Theory]
        [InlineData("pp", true)]   // Extra Small
        [InlineData("p", true)]    // Small
        [InlineData("m", true)]    // Medium
        [InlineData("g", true)]    // Large
        [InlineData("gg", true)]   // Extra Large
        [InlineData("xg", true)]   // Extra Extra Large
        [InlineData("xxg", true)]  // Extra Extra Large
        [InlineData("xxxg", true)] // Extra Extra Large
        [InlineData("plus_size", true)] // Plus Size
        [InlineData("invalid_size", false)]
        public void FashionSize_TryFromName_ShouldReturnCorrectResult(string name, bool expectedResult)
        {
            // Act
            var result = FashionSize.TryFromName(name, ignoreCase: true, out var fashionSize);

            // Assert
            Assert.Equal(expectedResult, result);
            if (expectedResult)
            {
                Assert.NotNull(fashionSize);
                Assert.Equal(name, fashionSize!.Name);
            }
            else
            {
                Assert.Null(fashionSize);
            }
        }

        [Fact]
        public void FashionSize_List_ShouldContainAllSizes()
        {
            // Arrange
            var expectedCount = 9;

            // Act
            var allFashionSizes = FashionSize.List;

            // Assert
            Assert.Equal(expectedCount, allFashionSizes.Count);
            Assert.Contains(FashionSize.PP, allFashionSizes);
            Assert.Contains(FashionSize.P, allFashionSizes);
            Assert.Contains(FashionSize.M, allFashionSizes);
            Assert.Contains(FashionSize.G, allFashionSizes);
            Assert.Contains(FashionSize.GG, allFashionSizes);
            Assert.Contains(FashionSize.XG, allFashionSizes);
            Assert.Contains(FashionSize.XXG, allFashionSizes);
            Assert.Contains(FashionSize.XXXG, allFashionSizes);
            Assert.Contains(FashionSize.PlusSize, allFashionSizes);
        }

        [Fact]
        public void FashionSize_InvalidFromValue_ShouldThrowException()
        {
            // Act & Assert
            Assert.Throws<SmartEnumNotFoundException>(() => FashionSize.FromValue(99));
        }
    }

}
