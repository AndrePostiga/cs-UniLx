using Ardalis.SmartEnum;
using UniLx.Domain.Entities.AdvertisementAgg.Enumerations;
using Xunit;

namespace UniLx.Tests.Domain.AdvertisementAgg.Enumerations
{ 
    public class AdvertisementTypeTests
    {
        [Theory]
        [InlineData("pets", 1)]
        [InlineData("beauty", 2)]
        [InlineData("electronics", 3)]
        [InlineData("events", 4)]
        [InlineData("fashion", 5)]
        [InlineData("real_estate", 6)]
        [InlineData("services", 7)]
        [InlineData("vehicles", 8)]
        [InlineData("job_opportunities", 9)]
        [InlineData("toys", 10)]
        [InlineData("others", 11)]
        public void AdvertisementType_ShouldHaveCorrectNameAndValue(string expectedName, int expectedValue)
        {
            // Act
            var advertisementType = AdvertisementType.List.FirstOrDefault(type => type.Value == expectedValue);

            // Assert
            Assert.NotNull(advertisementType);
            Assert.Equal(expectedName, advertisementType!.Name);
            Assert.Equal(expectedValue, advertisementType!.Value);
        }

        [Theory]
        [InlineData("pets", true)]
        [InlineData("unknown_type", false)]
        public void AdvertisementType_TryFromName_ShouldReturnCorrectResult(string name, bool expectedResult)
        {
            // Act
            var result = AdvertisementType.TryFromName(name, ignoreCase: true, out var advertisementType);

            // Assert
            Assert.Equal(expectedResult, result);
            if (expectedResult)
            {
                Assert.NotNull(advertisementType);
                Assert.Equal(name, advertisementType!.Name);
            }
            else
            {
                Assert.Null(advertisementType);
            }
        }

        [Fact]
        public void AdvertisementType_List_ShouldContainAllTypes()
        {
            // Arrange
            var expectedCount = 11;

            // Act
            var allTypes = AdvertisementType.List;

            // Assert
            Assert.Equal(expectedCount, allTypes.Count);
            Assert.Contains(AdvertisementType.Pets, allTypes);
            Assert.Contains(AdvertisementType.Beauty, allTypes);
            Assert.Contains(AdvertisementType.Electronics, allTypes);
            Assert.Contains(AdvertisementType.Events, allTypes);
            Assert.Contains(AdvertisementType.Fashion, allTypes);
            Assert.Contains(AdvertisementType.RealEstate, allTypes);
            Assert.Contains(AdvertisementType.Services, allTypes);
            Assert.Contains(AdvertisementType.Vehicles, allTypes);
            Assert.Contains(AdvertisementType.JobOpportunities, allTypes);
            Assert.Contains(AdvertisementType.Toys, allTypes);
            Assert.Contains(AdvertisementType.Others, allTypes);
        }

        [Theory]
        [InlineData(1, "pets")]
        [InlineData(5, "fashion")]
        [InlineData(11, "others")]
        public void AdvertisementType_FromValue_ShouldReturnCorrectType(int value, string expectedName)
        {
            // Act
            var advertisementType = AdvertisementType.FromValue(value);

            // Assert
            Assert.NotNull(advertisementType);
            Assert.Equal(expectedName, advertisementType.Name);
            Assert.Equal(value, advertisementType.Value);
        }

        [Fact]
        public void AdvertisementType_InvalidFromValue_ShouldThrowException()
        {
            // Act & Assert
            Assert.Throws<SmartEnumNotFoundException>(() => AdvertisementType.FromValue(99));
        }
    }

}
