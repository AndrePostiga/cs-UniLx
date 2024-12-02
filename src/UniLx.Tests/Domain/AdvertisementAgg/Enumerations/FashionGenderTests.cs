using Ardalis.SmartEnum;
using UniLx.Domain.Entities.AdvertisementAgg.Enumerations;
using Xunit;

namespace UniLx.Tests.Domain.AdvertisementAgg.Enumerations
{ 
    public class FashionGenderTests
    {
        [Theory]
        [InlineData("male", 1)]
        [InlineData("female", 2)]
        [InlineData("unisex", 3)]
        public void FashionGender_ShouldHaveCorrectNameAndValue(string expectedName, int expectedValue)
        {
            // Act
            var fashionGender = FashionGender.List.FirstOrDefault(fg => fg.Value == expectedValue);

            // Assert
            Assert.NotNull(fashionGender);
            Assert.Equal(expectedName, fashionGender!.Name);
            Assert.Equal(expectedValue, fashionGender!.Value);
        }

        [Theory]
        [InlineData("male", true)]
        [InlineData("female", true)]
        [InlineData("unisex", true)]
        [InlineData("invalid_gender", false)]
        public void FashionGender_TryFromName_ShouldReturnCorrectResult(string name, bool expectedResult)
        {
            // Act
            var result = FashionGender.TryFromName(name, ignoreCase: true, out var fashionGender);

            // Assert
            Assert.Equal(expectedResult, result);
            if (expectedResult)
            {
                Assert.NotNull(fashionGender);
                Assert.Equal(name, fashionGender!.Name);
            }
            else
            {
                Assert.Null(fashionGender);
            }
        }

        [Fact]
        public void FashionGender_List_ShouldContainAllTypes()
        {
            // Arrange
            var expectedCount = 3;

            // Act
            var allFashionGenders = FashionGender.List;

            // Assert
            Assert.Equal(expectedCount, allFashionGenders.Count);
            Assert.Contains(FashionGender.Male, allFashionGenders);
            Assert.Contains(FashionGender.Female, allFashionGenders);
            Assert.Contains(FashionGender.Unisex, allFashionGenders);
        }

        [Theory]
        [InlineData(1, "male")]
        [InlineData(2, "female")]
        [InlineData(3, "unisex")]
        public void FashionGender_FromValue_ShouldReturnCorrectType(int value, string expectedName)
        {
            // Act
            var fashionGender = FashionGender.FromValue(value);

            // Assert
            Assert.NotNull(fashionGender);
            Assert.Equal(expectedName, fashionGender.Name);
            Assert.Equal(value, fashionGender.Value);
        }

        [Fact]
        public void FashionGender_InvalidFromValue_ShouldThrowException()
        {
            // Act & Assert
            Assert.Throws<SmartEnumNotFoundException>(() => FashionGender.FromValue(99));
        }
    }

}
