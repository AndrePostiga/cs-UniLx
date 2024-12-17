using Ardalis.SmartEnum;
using UniLx.Domain.Entities.AdvertisementAgg.Enumerations;

namespace UniLx.Tests.Domain.AdvertisementAgg.Enumerations
{
    public class PropertyConditionTests
    {
        [Theory]
        [InlineData("new", 1)]
        [InlineData("renovated", 2)]
        [InlineData("used", 3)]
        public void PropertyCondition_ShouldHaveCorrectNameAndValue(string expectedName, int expectedValue)
        {
            // Act
            var condition = PropertyCondition.List.FirstOrDefault(c => c.Value == expectedValue);

            // Assert
            Assert.NotNull(condition);
            Assert.Equal(expectedName, condition!.Name);
            Assert.Equal(expectedValue, condition!.Value);
        }

        [Theory]
        [InlineData("new", true)]
        [InlineData("used", true)]
        [InlineData("unknown", false)]
        public void PropertyCondition_TryFromName_ShouldReturnCorrectResult(string name, bool expectedResult)
        {
            // Act
            var result = PropertyCondition.TryFromName(name, ignoreCase: true, out var condition);

            // Assert
            Assert.Equal(expectedResult, result);
            if (expectedResult)
            {
                Assert.NotNull(condition);
                Assert.Equal(name, condition!.Name);
            }
            else
            {
                Assert.Null(condition);
            }
        }

        [Fact]
        public void PropertyCondition_List_ShouldContainAllTypes()
        {
            // Arrange
            var expectedCount = 3;

            // Act
            var allConditions = PropertyCondition.List;

            // Assert
            Assert.Equal(expectedCount, allConditions.Count);
            Assert.Contains(PropertyCondition.New, allConditions);
            Assert.Contains(PropertyCondition.Renovated, allConditions);
            Assert.Contains(PropertyCondition.Used, allConditions);
        }

        [Theory]
        [InlineData(1, "new")]
        [InlineData(3, "used")]
        public void PropertyCondition_FromValue_ShouldReturnCorrectType(int value, string expectedName)
        {
            // Act
            var condition = PropertyCondition.FromValue(value);

            // Assert
            Assert.NotNull(condition);
            Assert.Equal(expectedName, condition.Name);
            Assert.Equal(value, condition.Value);
        }

        [Fact]
        public void PropertyCondition_InvalidFromValue_ShouldThrowException()
        {
            // Act & Assert
            Assert.Throws<SmartEnumNotFoundException>(() => PropertyCondition.FromValue(99));
        }
    }
}
