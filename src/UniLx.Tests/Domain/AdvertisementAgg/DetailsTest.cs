using UniLx.Domain.Entities.AdvertisementAgg;
using UniLx.Domain.Entities.AdvertisementAgg.Enumerations;
using UniLx.Domain.Exceptions;

namespace UniLx.Tests.Domain.AdvertisementAgg
{
    public class DetailsTests
    {
        private class TestDetailsStub : Details
        {
            protected override AdvertisementType Type => AdvertisementType.Services;

            public TestDetailsStub(string? title, string? description, int? price)
                : base(title, description, price) { }
        }

        [Fact]
        public void Details_Should_Set_Valid_Title_Description_And_Price()
        {
            // Arrange
            var title = "Valid Title";
            var description = "This is a valid description.";
            var price = 500;

            // Act
            var details = new TestDetailsStub(title, description, price);

            // Assert
            Assert.Equal(title, details.Title);
            Assert.Equal(WebUtility.HtmlEncode(description), details.Description);
            Assert.Equal(price, details.Price);
        }

        [Fact]
        public void Details_Should_Throw_When_Title_Is_Null_Or_Empty()
        {
            // Arrange
            var invalidTitle = "";

            // Act & Assert
            Assert.Throws<DomainException>(() => new TestDetailsStub(invalidTitle, "Description", 100));
        }

        [Fact]
        public void Details_Should_Throw_When_Title_Exceeds_Max_Length()
        {
            // Arrange
            var invalidTitle = new string('A', 257);

            // Act & Assert
            Assert.Throws<DomainException>(() => new TestDetailsStub(invalidTitle, "Description", 100));
        }

        [Fact]
        public void Details_Should_Throw_When_Description_Exceeds_Max_Length()
        {
            // Arrange
            var invalidDescription = new string('A', 513);

            // Act & Assert
            Assert.Throws<DomainException>(() => new TestDetailsStub("Title", invalidDescription, 100));
        }

        [Fact]
        public void Details_Should_Allow_Null_Description()
        {
            // Arrange
            string? description = null;

            // Act
            var details = new TestDetailsStub("Title", description, 100);

            // Assert
            Assert.Null(details.Description);
        }

        [Fact]
        public void Details_Should_Throw_When_Price_Is_Negative()
        {
            // Arrange
            var negativePrice = -100;

            // Act & Assert
            Assert.Throws<DomainException>(() => new TestDetailsStub("Title", "Description", negativePrice));
        }

        [Fact]
        public void Details_Should_Throw_When_Price_Exceeds_Max_Value()
        {
            // Arrange
            var excessivePrice = 100_000_001;

            // Act & Assert
            Assert.Throws<DomainException>(() => new TestDetailsStub("Title", "Description", excessivePrice));
        }

        [Fact]
        public void Details_Should_Allow_Null_Price()
        {
            // Arrange
            int? price = null;

            // Act
            var details = new TestDetailsStub("Title", "Description", price);

            // Assert
            Assert.Null(details.Price);
        }

        [Fact]
        public void GetType_Should_Return_Correct_AdvertisementType()
        {
            // Arrange
            var details = new TestDetailsStub("Title", "Description", 100);

            // Act
            var type = details.GetType();

            // Assert
            Assert.Equal(AdvertisementType.Services, type);
        }
    }

}
