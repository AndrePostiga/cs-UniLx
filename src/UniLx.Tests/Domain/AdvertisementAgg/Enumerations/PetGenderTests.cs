using Ardalis.SmartEnum;
using UniLx.Domain.Entities.AdvertisementAgg.Enumerations;

namespace UniLx.Tests.Domain.AdvertisementAgg.Enumerations
{
    public class PetGenderTests
    {
        [Theory]
        [InlineData("male", 1)]
        [InlineData("female", 2)]
        public void PetGender_ShouldHaveCorrectNameAndValue(string expectedName, int expectedValue)
        {
            // Act
            var petGender = PetGender.List.FirstOrDefault(pg => pg.Value == expectedValue && pg.Name == expectedName);

            // Assert
            Assert.NotNull(petGender);
            Assert.Equal(expectedName, petGender!.Name);
            Assert.Equal(expectedValue, petGender!.Value);
        }

        [Theory]
        [InlineData("male", true)]
        [InlineData("female", true)]
        [InlineData("unknown", false)]
        public void PetGender_TryFromName_ShouldReturnCorrectResult(string name, bool expectedResult)
        {
            // Act
            var result = PetGender.TryFromName(name, ignoreCase: true, out var petGender);

            // Assert
            Assert.Equal(expectedResult, result);
            if (expectedResult)
            {
                Assert.NotNull(petGender);
                Assert.Equal(name, petGender!.Name);
            }
            else
            {
                Assert.Null(petGender);
            }
        }

        [Fact]
        public void PetGender_List_ShouldContainBothGenders()
        {
            // Arrange
            var expectedCount = 2;

            // Act
            var allPetGenders = PetGender.List;

            // Assert
            Assert.Equal(expectedCount, allPetGenders.Count);
            Assert.Contains(PetGender.Male, allPetGenders);
            Assert.Contains(PetGender.Female, allPetGenders);
        }

        [Theory]
        [InlineData(1, "male")]
        [InlineData(2, "female")]
        public void PetGender_FromValue_ShouldReturnCorrectType(int value, string expectedName)
        {
            // Act
            var petGender = PetGender.FromValue(value);

            // Assert
            Assert.NotNull(petGender);
            Assert.Equal(expectedName, petGender.Name);
            Assert.Equal(value, petGender.Value);
        }

        [Fact]
        public void PetGender_InvalidFromValue_ShouldThrowException()
        {
            // Act & Assert
            Assert.Throws<SmartEnumNotFoundException>(() => PetGender.FromValue(99));
        }
    }

}
