
using Ardalis.SmartEnum;
using UniLx.Domain.Entities.AdvertisementAgg.Enumerations;

namespace UniLx.Tests.Domain.AdvertisementAgg.Enumerations
{
    public class WorkLocationTypeTests
    {
        [Theory]
        [InlineData("remote", 1)]
        [InlineData("on_site", 2)]
        [InlineData("hybrid", 3)]
        public void WorkLocationType_ShouldHaveCorrectNameAndValue(string expectedName, int expectedValue)
        {
            // Act
            var workLocationType = WorkLocationType.List.FirstOrDefault(wlt => wlt.Value == expectedValue && wlt.Name == expectedName);

            // Assert
            Assert.NotNull(workLocationType);
            Assert.Equal(expectedName, workLocationType!.Name);
            Assert.Equal(expectedValue, workLocationType!.Value);
        }

        [Theory]
        [InlineData("remote", true)]
        [InlineData("on_site", true)]
        [InlineData("hybrid", true)]
        [InlineData("unknown", false)]
        public void WorkLocationType_TryFromName_ShouldReturnCorrectResult(string name, bool expectedResult)
        {
            // Act
            var result = WorkLocationType.TryFromName(name, ignoreCase: true, out var workLocationType);

            // Assert
            Assert.Equal(expectedResult, result);
            if (expectedResult)
            {
                Assert.NotNull(workLocationType);
                Assert.Equal(name, workLocationType!.Name);
            }
            else
            {
                Assert.Null(workLocationType);
            }
        }

        [Fact]
        public void WorkLocationType_List_ShouldContainAllTypes()
        {
            // Arrange
            var expectedCount = 3;

            // Act
            var allWorkLocationTypes = WorkLocationType.List;

            // Assert
            Assert.Equal(expectedCount, allWorkLocationTypes.Count);
            Assert.Contains(WorkLocationType.Remote, allWorkLocationTypes);
            Assert.Contains(WorkLocationType.OnSite, allWorkLocationTypes);
            Assert.Contains(WorkLocationType.Hybrid, allWorkLocationTypes);
        }

        [Theory]
        [InlineData(1, "remote")]
        [InlineData(2, "on_site")]
        [InlineData(3, "hybrid")]
        public void WorkLocationType_FromValue_ShouldReturnCorrectType(int value, string expectedName)
        {
            // Act
            var workLocationType = WorkLocationType.FromValue(value);

            // Assert
            Assert.NotNull(workLocationType);
            Assert.Equal(expectedName, workLocationType.Name);
            Assert.Equal(value, workLocationType.Value);
        }

        [Fact]
        public void WorkLocationType_InvalidFromValue_ShouldThrowException()
        {
            // Act & Assert
            Assert.Throws<SmartEnumNotFoundException>(() => WorkLocationType.FromValue(99));
        }
    }

}
