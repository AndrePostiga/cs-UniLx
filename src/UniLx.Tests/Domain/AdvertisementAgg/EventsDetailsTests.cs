using UniLx.Domain.Entities.AdvertisementAgg.Enumerations;
using UniLx.Domain.Entities.AdvertisementAgg.SpecificDetails;
using UniLx.Domain.Entities.Seedwork.ValueObj;
using UniLx.Domain.Exceptions;

namespace UniLx.Tests.Domain.AdvertisementAgg
{
    public class EventsDetailsTests
    {
        [Theory]
        [InlineData("free")]
        [InlineData("age10")]
        [InlineData("age12")]
        [InlineData("age14")]
        [InlineData("age16")]
        [InlineData("age18")]
        public void EventsDetails_Should_Create_Valid_Instance_With_AgeRestriction(string ageRestrictionName)
        {
            // Arrange
            var title = "Music Festival";
            var description = "An exciting outdoor music festival.";
            var price = 150;
            var eventType = "Concert";
            var eventDate = DateTime.UtcNow.AddDays(10);
            var organizer = "Event Co.";
            var dressCode = "Casual";
            var highlights = new List<string> { "Live Performances", "Food Stalls" };
            var isOnline = false;
            var contactInfo = new ContactInformation(new Phone("55", "21", "987654321"), new Email("info@example.com"), "https://example.com");

            // Act
            var details = new EventsDetails(
                title, description, price, eventType, eventDate, organizer,
                ageRestrictionName, dressCode, highlights, isOnline, contactInfo);

            // Assert
            Assert.NotNull(details);
            Assert.Equal(title, details.Title);
            Assert.Equal(description, details.Description);
            Assert.Equal(price, details.Price);
            Assert.Equal(eventType, details.EventType);
            Assert.Equal(eventDate, details.EventDate);
            Assert.Equal(organizer, details.Organizer);
            Assert.Equal(AgeRestriction.FromName(ageRestrictionName, ignoreCase: true), details.AgeRestriction);
            Assert.Equal(dressCode, details.DressCode);
            Assert.Equal(highlights, details.Highlights);
            Assert.Equal(isOnline, details.IsOnline);
            Assert.Equal(contactInfo, details.ContactInfo);
        }

        [Fact]
        public void EventsDetails_Should_Throw_When_EventType_Is_Null_Or_Empty()
        {
            // Arrange
            string? invalidEventType = null;

            // Act & Assert
            Assert.Throws<DomainException>(() =>
                new EventsDetails("Title", "Description", 100, invalidEventType, DateTime.UtcNow.AddDays(1), null, "free", null, null, false, null));
        }

        [Fact]
        public void EventsDetails_Should_Throw_When_EventType_Exceeds_Max_Length()
        {
            // Arrange
            var invalidEventType = new string('A', 101);

            // Act & Assert
            Assert.Throws<DomainException>(() =>
                new EventsDetails("Title", "Description", 100, invalidEventType, DateTime.UtcNow.AddDays(1), null, "free", null, null, false, null));
        }

        [Fact]
        public void EventsDetails_Should_Throw_When_EventDate_Is_In_The_Past()
        {
            // Arrange
            var pastDate = DateTime.UtcNow.AddDays(-1);

            // Act & Assert
            Assert.Throws<DomainException>(() =>
                new EventsDetails("Title", "Description", 100, "Concert", pastDate, null, "free", null, null, false, null));
        }

        [Fact]
        public void EventsDetails_Should_Throw_When_AgeRestriction_Is_Invalid()
        {
            // Arrange
            var invalidAgeRestriction = "invalid";

            // Act & Assert
            Assert.Throws<DomainException>(() =>
                new EventsDetails("Title", "Description", 100, "Concert", DateTime.UtcNow.AddDays(1), null, invalidAgeRestriction, null, null, false, null));
        }

        [Fact]
        public void EventsDetails_Should_Throw_When_ContactInformation_Is_Null()
        {
            // Act & Assert
            Assert.Throws<DomainException>(() =>
                new EventsDetails("Title", "Description", 100, "Concert", DateTime.UtcNow.AddDays(1), null, "free", null, null, false, null));
        }

        [Fact]
        public void EventsDetails_Should_Throw_When_Highlight_Exceeds_Max_Length()
        {
            // Arrange
            var invalidHighlights = new List<string> { "Valid Highlight", new string('A', 101) };

            // Act & Assert
            Assert.Throws<DomainException>(() =>
                new EventsDetails("Title", "Description", 100, "Concert", DateTime.UtcNow.AddDays(1), null, "free", null, invalidHighlights, false, null));
        }
    }
}

