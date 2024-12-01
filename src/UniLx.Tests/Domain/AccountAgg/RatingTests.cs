using UniLx.Domain.Entities.AccountAgg;
using UniLx.Domain.Exceptions;

namespace UniLx.Tests.Domain.AccountAgg
{
    public class RatingTests
    {
        [Fact]
        public void Constructor_ShouldInitializeWithDefaultValues()
        {
            // Act
            var rating = new Rating();

            // Assert
            Assert.Equal(0, rating.Value);
            Assert.Equal(0, rating.Count);
            Assert.NotNull(rating.UpdatedAt);
        }

        [Fact]
        public void UpdateRating_ValidValue_ShouldUpdateRatingCorrectly()
        {
            // Arrange
            var rating = new Rating();
            var firstValue = 4.0f;
            var secondValue = 3.0f;

            // Act
            rating.UpdateRating(firstValue);
            rating.UpdateRating(secondValue);

            // Assert
            Assert.Equal(3.5f, rating.Value); // Average of 4.0 and 3.0
            Assert.Equal(2, rating.Count);
            Assert.NotNull(rating.UpdatedAt);
        }

        [Fact]
        public void UpdateRating_ValueBelowZero_ShouldThrowException()
        {
            // Arrange
            var rating = new Rating();

            // Act & Assert
            var exception = Assert.Throws<DomainException>(() => rating.UpdateRating(-1.0f));
            Assert.Equal("Rating must be greather than 0.", exception.Message);
        }

        [Fact]
        public void UpdateRating_ValueAboveMax_ShouldThrowException()
        {
            // Arrange
            var rating = new Rating();

            // Act & Assert
            var exception = Assert.Throws<DomainException>(() => rating.UpdateRating(6.0f));
            Assert.Equal($"Rating must be lower than {5.0f}.", exception.Message);
        }

        [Fact]
        public void UpdateRating_ShouldUpdateUpdatedAtToCurrentTime()
        {
            // Arrange
            var rating = new Rating();
            var initialUpdatedAt = rating.UpdatedAt;

            // Act
            rating.UpdateRating(4.5f);

            // Assert
            Assert.NotEqual(initialUpdatedAt, rating.UpdatedAt);
            Assert.True(rating.UpdatedAt > initialUpdatedAt);
        }
    }
}
