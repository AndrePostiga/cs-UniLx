using Ardalis.SmartEnum;
using UniLx.Domain.Entities.AdvertisementAgg.Enumerations;

namespace UniLx.Tests.Domain.AdvertisementAgg.Enumerations
{
    public class PetTypeTests
    {
        [Theory]
        [InlineData("sell", 1)]
        [InlineData("adoption", 2)]
        [InlineData("accessories", 3)]
        public void PetType_ShouldHaveCorrectNameAndValue(string expectedName, int expectedValue)
        {
            // Act
            var petType = PetType.List.FirstOrDefault(pt => pt.Value == expectedValue && pt.Name == expectedName);

            // Assert
            Assert.NotNull(petType);
            Assert.Equal(expectedName, petType!.Name);
            Assert.Equal(expectedValue, petType!.Value);
        }

        [Theory]
        [InlineData("sell", true)]
        [InlineData("adoption", true)]
        [InlineData("accessories", true)]
        [InlineData("unknown", false)]
        public void PetType_TryFromName_ShouldReturnCorrectResult(string name, bool expectedResult)
        {
            // Act
            var result = PetType.TryFromName(name, ignoreCase: true, out var petType);

            // Assert
            Assert.Equal(expectedResult, result);
            if (expectedResult)
            {
                Assert.NotNull(petType);
                Assert.Equal(name, petType!.Name);
            }
            else
            {
                Assert.Null(petType);
            }
        }

        [Fact]
        public void PetType_List_ShouldContainAllTypes()
        {
            // Arrange
            var expectedCount = 3;

            // Act
            var allPetTypes = PetType.List;

            // Assert
            Assert.Equal(expectedCount, allPetTypes.Count);
            Assert.Contains(PetType.Sell, allPetTypes);
            Assert.Contains(PetType.Adoption, allPetTypes);
            Assert.Contains(PetType.Accessories, allPetTypes);
        }

        [Theory]
        [InlineData(1, "sell")]
        [InlineData(2, "adoption")]
        [InlineData(3, "accessories")]
        public void PetType_FromValue_ShouldReturnCorrectType(int value, string expectedName)
        {
            // Act
            var petType = PetType.FromValue(value);

            // Assert
            Assert.NotNull(petType);
            Assert.Equal(expectedName, petType.Name);
            Assert.Equal(value, petType.Value);
        }

        [Fact]
        public void PetType_InvalidFromValue_ShouldThrowException()
        {
            // Act & Assert
            Assert.Throws<SmartEnumNotFoundException>(() => PetType.FromValue(99));
        }
    }

}
