using System.Linq.Expressions;
using UniLx.Shared.LibExtensions;

namespace UniLx.Tests.Shared.LibExtensions
{
    public class LambdaExtensionsTests
    {
        [Fact]
        public void And_ShouldCombineTwoExpressionsWithAndAlso()
        {
            // Arrange
            Expression<Func<int, bool>> isEven = x => x % 2 == 0;
            Expression<Func<int, bool>> isPositive = x => x > 0;

            // Act
            var combinedExpression = isEven.And(isPositive);
            var compiled = combinedExpression.Compile();

            // Assert
            Assert.True(compiled(2));  // Even and positive
            Assert.False(compiled(-2)); // Even but not positive
            Assert.False(compiled(1));  // Positive but not even
            Assert.False(compiled(-1)); // Neither even nor positive
        }

        [Fact]
        public void Or_ShouldCombineTwoExpressionsWithOrElse()
        {
            // Arrange
            Expression<Func<int, bool>> isNegative = x => x < 0;
            Expression<Func<int, bool>> isZero = x => x == 0;

            // Act
            var combinedExpression = isNegative.Or(isZero);
            var compiled = combinedExpression.Compile();

            // Assert
            Assert.True(compiled(-1)); // Negative
            Assert.True(compiled(0));  // Zero
            Assert.False(compiled(1)); // Positive and not zero
        }

        [Fact]
        public void And_ShouldPreserveParameterType()
        {
            // Arrange
            Expression<Func<int, bool>> isEven = x => x % 2 == 0;
            Expression<Func<int, bool>> isPositive = y => y > 0;

            // Act
            var combinedExpression = isEven.And(isPositive);

            // Assert
            Assert.NotNull(combinedExpression.Parameters[0]); // Ensure parameter exists
            Assert.Equal(typeof(int), combinedExpression.Parameters[0].Type); // Check parameter type
        }

        [Fact]
        public void Or_ShouldPreserveParameterType()
        {
            // Arrange
            Expression<Func<int, bool>> isNegative = x => x < 0;
            Expression<Func<int, bool>> isZero = y => y == 0;

            // Act
            var combinedExpression = isNegative.Or(isZero);

            // Assert
            Assert.NotNull(combinedExpression.Parameters[0]); // Ensure parameter exists
            Assert.Equal(typeof(int), combinedExpression.Parameters[0].Type); // Check parameter type
        }

        [Fact]
        public void CombinedExpression_ShouldWorkForComplexExpressions()
        {
            // Arrange
            Expression<Func<string, bool>> startsWithA = s => s.StartsWith("A");
            Expression<Func<string, bool>> endsWithZ = s => s.EndsWith("Z");

            // Act
            var combinedExpression = startsWithA.And(endsWithZ);
            var compiled = combinedExpression.Compile();

            // Assert
            Assert.True(compiled("AZ"));   // Starts with A and ends with Z
            Assert.False(compiled("A"));   // Starts with A but doesn't end with Z
            Assert.False(compiled("Z"));   // Ends with Z but doesn't start with A
            Assert.False(compiled("XYZ")); // Neither starts with A nor ends with Z
        }
    }
}


