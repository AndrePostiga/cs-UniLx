using System.Reflection;
using UniLx.Domain.Entities.Seedwork.ValueObj;
using UniLx.Domain.Exceptions;

namespace UniLx.Tests.Domain.Seedwork.ValueObj
{
    public class EmailTests
    {
        [Fact]
        public void Email_Private_Default_Constructor_Should_Work()
        {
            // Act
            var privateConstructor = typeof(Email).GetConstructor(
                BindingFlags.NonPublic | BindingFlags.Instance,
                null,
                Type.EmptyTypes,
                null);

            var email = (Email)privateConstructor!.Invoke(null);

            // Assert
            Assert.NotNull(email);
            Assert.Null(email.Value);
        }

        [Fact]
        public void Email_Should_Create_Valid_Email()
        {
            // Arrange
            var validEmail = "test@example.com";

            // Act
            var email = new Email(validEmail);

            // Assert
            Assert.NotNull(email);
            Assert.Equal(validEmail, email.Value);
        }

        [Fact]
        public void Email_Should_Throw_On_Empty_Email()
        {
            // Arrange
            var emptyEmail = "";

            // Act & Assert
            Assert.Throws<DomainException>(() =>
            {
                new Email(emptyEmail);
            });
        }

        [Fact]
        public void Email_Should_Throw_On_Invalid_Email_Format()
        {
            // Arrange
            var invalidEmail = "invalid-email";

            // Act & Assert
            Assert.Throws<DomainException>(() =>
            {
                new Email(invalidEmail);
            });
        }

        [Fact]
        public void Email_ToString_Should_Return_Email_Value()
        {
            // Arrange
            var validEmail = "test@example.com";
            var email = new Email(validEmail);

            // Act
            var result = email.ToString();

            // Assert
            Assert.Equal(validEmail, result);
        }
    }
}
