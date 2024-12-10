using System.Reflection;
using UniLx.Domain.Entities.Seedwork.ValueObj;
using UniLx.Domain.Exceptions;

namespace UniLx.Tests.Domain.Seedwork.ValueObj
{
    public class ContactInformationTests
    {
        [Fact]
        public void ContactInformation_Should_Be_Created_With_Private_Default_Constructor()
        {
            // Act
            var privateConstructor = typeof(ContactInformation)
                .GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, Type.EmptyTypes, null);

            var contactInfo = (ContactInformation)privateConstructor!.Invoke(null);

            // Assert
            Assert.NotNull(contactInfo);
            Assert.Null(contactInfo.Phone);
            Assert.Null(contactInfo.Email);
            Assert.Null(contactInfo.Website);
        }

        [Fact]
        public void ContactInformation_Should_Create_With_Valid_Phone_Only()
        {
            // Arrange
            var phone = new Phone("55", "21", "987654321");

            // Act
            var contactInfo = new ContactInformation(phone, null, null);

            // Assert
            Assert.NotNull(contactInfo);
            Assert.Equal(phone, contactInfo.Phone);
            Assert.Null(contactInfo.Email);
            Assert.Null(contactInfo.Website);
        }

        [Fact]
        public void ContactInformation_Should_Create_With_Valid_Email_Only()
        {
            // Arrange
            var email = new Email("test@example.com");

            // Act
            var contactInfo = new ContactInformation(null, email, null);

            // Assert
            Assert.NotNull(contactInfo);
            Assert.Null(contactInfo.Phone);
            Assert.Equal(email, contactInfo.Email);
            Assert.Null(contactInfo.Website);
        }

        [Fact]
        public void ContactInformation_Should_Create_With_Valid_Website()
        {
            // Arrange
            var phone = new Phone("55", "21", "987654321");
            var email = new Email("test@example.com");
            var website = "https://example.com";

            // Act
            var contactInfo = new ContactInformation(phone, email, website);

            // Assert
            Assert.NotNull(contactInfo);
            Assert.Equal(phone, contactInfo.Phone);
            Assert.Equal(email, contactInfo.Email);
            Assert.NotNull(contactInfo.Website);
            Assert.Equal(new Uri(website), contactInfo.Website);
        }

        [Fact]
        public void ContactInformation_Should_Throw_When_No_Phone_Or_Email_Provided()
        {
            // Arrange
            var website = "https://example.com";

            // Act & Assert
            Assert.Throws<DomainException>(() =>
            {
                new ContactInformation(null, null, website);
            });
        }

        [Fact]
        public void ContactInformation_Should_Throw_On_Invalid_Website()
        {
            // Arrange
            var phone = new Phone("55", "21", "987654321");
            var email = new Email("test@example.com");
            var invalidWebsite = "invalid-url";

            // Act & Assert
            Assert.Throws<DomainException>(() =>
            {
                new ContactInformation(phone, email, invalidWebsite);
            });
        }

        [Fact]
        public void ContactInformation_ToString_Should_Include_All_Provided_Information()
        {
            // Arrange
            var phone = new Phone("55", "21", "987654321");
            var email = new Email("test@example.com");
            var website = "https://example.com";

            // Act
            var contactInfo = new ContactInformation(phone, email, website);
            var result = contactInfo.ToString();

            // Assert
            Assert.Contains($"Phone: {phone}", result);
            Assert.Contains($"Email: {email}", result);
            Assert.Contains($"Website: {website}", result);
        }

        [Fact]
        public void ContactInformation_ToString_Should_Indicate_Missing_Phone()
        {
            // Arrange
            var email = new Email("test@example.com");
            var website = "https://example.com";

            // Act
            var contactInfo = new ContactInformation(null, email, website);
            var result = contactInfo.ToString();

            // Assert
            Assert.Contains("Phone: Not provided", result);
            Assert.Contains($"Email: {email}", result);
            Assert.Contains($"Website: {website}", result);
        }

        [Fact]
        public void ContactInformation_ToString_Should_Indicate_Missing_Email()
        {
            // Arrange
            var phone = new Phone("55", "21", "987654321");
            var website = "https://example.com";

            // Act
            var contactInfo = new ContactInformation(phone, null, website);
            var result = contactInfo.ToString();

            // Assert
            Assert.Contains($"Phone: {phone}", result);
            Assert.Contains("Email: Not provided", result);
            Assert.Contains($"Website: {website}", result);
        }

        [Fact]
        public void ContactInformation_ToString_Should_Indicate_Missing_Website()
        {
            // Arrange
            var phone = new Phone("55", "21", "987654321");
            var email = new Email("test@example.com");

            // Act
            var contactInfo = new ContactInformation(phone, email, null);
            var result = contactInfo.ToString();

            // Assert
            Assert.Contains($"Phone: {phone}", result);
            Assert.Contains($"Email: {email}", result);
            Assert.Contains("Website: Not provided", result);
        }
    }
}
