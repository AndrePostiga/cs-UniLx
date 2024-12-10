using Ardalis.SmartEnum;
using UniLx.Domain.Entities.AdvertisementAgg.Enumerations;
using Xunit;


namespace UniLx.Tests.Domain.AdvertisementAgg.Enumerations
{    
    public class EmploymentTypeTests
    {
        [Theory]
        [InlineData("full_time", 1)]
        [InlineData("part_time", 2)]
        [InlineData("contract", 3)]
        public void EmploymentType_ShouldHaveCorrectNameAndValue(string expectedName, int expectedValue)
        {
            // Act
            var employmentType = EmploymentType.List.FirstOrDefault(et => et.Value == expectedValue);

            // Assert
            Assert.NotNull(employmentType);
            Assert.Equal(expectedName, employmentType!.Name);
            Assert.Equal(expectedValue, employmentType!.Value);
        }

        [Theory]
        [InlineData("full_time", true)]
        [InlineData("part_time", true)]
        [InlineData("contract", true)]
        [InlineData("invalid_type", false)]
        public void EmploymentType_TryFromName_ShouldReturnCorrectResult(string name, bool expectedResult)
        {
            // Act
            var result = EmploymentType.TryFromName(name, ignoreCase: true, out var employmentType);

            // Assert
            Assert.Equal(expectedResult, result);
            if (expectedResult)
            {
                Assert.NotNull(employmentType);
                Assert.Equal(name, employmentType!.Name);
            }
            else
            {
                Assert.Null(employmentType);
            }
        }

        [Fact]
        public void EmploymentType_List_ShouldContainAllTypes()
        {
            // Arrange
            var expectedCount = 3;

            // Act
            var allEmploymentTypes = EmploymentType.List;

            // Assert
            Assert.Equal(expectedCount, allEmploymentTypes.Count);
            Assert.Contains(EmploymentType.FullTime, allEmploymentTypes);
            Assert.Contains(EmploymentType.PartTime, allEmploymentTypes);
            Assert.Contains(EmploymentType.Contract, allEmploymentTypes);
        }

        [Theory]
        [InlineData(1, "full_time")]
        [InlineData(2, "part_time")]
        [InlineData(3, "contract")]
        public void EmploymentType_FromValue_ShouldReturnCorrectType(int value, string expectedName)
        {
            // Act
            var employmentType = EmploymentType.FromValue(value);

            // Assert
            Assert.NotNull(employmentType);
            Assert.Equal(expectedName, employmentType.Name);
            Assert.Equal(value, employmentType.Value);
        }

        [Fact]
        public void EmploymentType_InvalidFromValue_ShouldThrowException()
        {
            // Act & Assert
            Assert.Throws<SmartEnumNotFoundException>(() => EmploymentType.FromValue(99));
        }
    }

}
