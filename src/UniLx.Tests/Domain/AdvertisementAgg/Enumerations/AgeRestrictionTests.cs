using Ardalis.SmartEnum;
using UniLx.Domain.Entities.AdvertisementAgg.Enumerations;

namespace UniLx.Tests.Domain.AdvertisementAgg.Enumerations
{
    public class AgeRestrictionTests
    {
        [Theory]
        [InlineData("free", 1)]
        [InlineData("age10", 2)]
        [InlineData("age12", 3)]
        [InlineData("age14", 4)]
        [InlineData("age16", 5)]
        [InlineData("age18", 6)]
        public void AgeRestriction_ShouldHaveCorrectNameAndValue(string expectedName, int expectedValue)
        {
            // Act
            var ageRestriction = AgeRestriction.List.FirstOrDefault(ar => ar.Value == expectedValue);

            // Assert
            Assert.NotNull(ageRestriction);
            Assert.Equal(expectedName, ageRestriction!.Name);
            Assert.Equal(expectedValue, ageRestriction!.Value);
        }

        [Theory]
        [InlineData("free", true)]
        [InlineData("age18", true)]
        [InlineData("invalid_age", false)]
        public void AgeRestriction_TryFromName_ShouldReturnCorrectResult(string name, bool expectedResult)
        {
            // Act
            var result = AgeRestriction.TryFromName(name, ignoreCase: true, out var ageRestriction);

            // Assert
            Assert.Equal(expectedResult, result);
            if (expectedResult)
            {
                Assert.NotNull(ageRestriction);
                Assert.Equal(name, ageRestriction!.Name);
            }
            else
            {
                Assert.Null(ageRestriction);
            }
        }

        [Fact]
        public void AgeRestriction_List_ShouldContainAllTypes()
        {
            // Arrange
            var expectedCount = 6;

            // Act
            var allAgeRestrictions = AgeRestriction.List;

            // Assert
            Assert.Equal(expectedCount, allAgeRestrictions.Count);
            Assert.Contains(AgeRestriction.Free, allAgeRestrictions);
            Assert.Contains(AgeRestriction.Age10, allAgeRestrictions);
            Assert.Contains(AgeRestriction.Age12, allAgeRestrictions);
            Assert.Contains(AgeRestriction.Age14, allAgeRestrictions);
            Assert.Contains(AgeRestriction.Age16, allAgeRestrictions);
            Assert.Contains(AgeRestriction.Age18, allAgeRestrictions);
        }

        [Theory]
        [InlineData(1, "free")]
        [InlineData(3, "age12")]
        [InlineData(6, "age18")]
        public void AgeRestriction_FromValue_ShouldReturnCorrectType(int value, string expectedName)
        {
            // Act
            var ageRestriction = AgeRestriction.FromValue(value);

            // Assert
            Assert.NotNull(ageRestriction);
            Assert.Equal(expectedName, ageRestriction.Name);
            Assert.Equal(value, ageRestriction.Value);
        }

        [Fact]
        public void AgeRestriction_InvalidFromValue_ShouldThrowException()
        {
            // Act & Assert
            Assert.Throws<SmartEnumNotFoundException>(() => AgeRestriction.FromValue(99));
        }
    }

}
