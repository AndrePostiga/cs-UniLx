using System.Reflection;
using UniLx.Domain.Entities.Seedwork.ValueObj;
using UniLx.Domain.Exceptions;

namespace UniLx.Tests.Domain.Seedwork.ValueObj
{
    public class StorageImageTests
    {
        [Fact]
        public void StorageImage_Private_Default_Constructor_Should_Work()
        {
            // Act
            var privateConstructor = typeof(StorageImage).GetConstructor(
                BindingFlags.NonPublic | BindingFlags.Instance,
                null,
                Type.EmptyTypes,
                null);

            var storageImage = (StorageImage)privateConstructor!.Invoke(null);

            // Assert
            Assert.NotNull(storageImage);
            Assert.Null(storageImage.AccessUrl);
            Assert.Null(storageImage.Path);
            Assert.Null(storageImage.FileName);
            Assert.Null(storageImage.FullPath);
            Assert.False(storageImage.IsPrivate);
            Assert.Null(storageImage.StorageType);
        }

        [Fact]
        public void ValidateImageFileName_Should_Return_True_For_Supported_Format()
        {
            // Arrange
            var validFileName = "image.jpg";

            // Act
            var result = StorageImage.ValidateImageFileName(validFileName);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ValidateImageFileName_Should_Return_False_For_Unsupported_Format()
        {
            // Arrange
            var unsupportedFileName = "image.txt";

            // Act
            var result = StorageImage.ValidateImageFileName(unsupportedFileName);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ValidateImageFileName_Should_Return_False_For_Null_Or_Empty_FileName()
        {
            // Arrange
            var nullFileName = (string?)null;
            var emptyFileName = "";

            // Act
            var resultForNull = StorageImage.ValidateImageFileName(nullFileName);
            var resultForEmpty = StorageImage.ValidateImageFileName(emptyFileName);

            // Assert
            Assert.False(resultForNull);
            Assert.False(resultForEmpty);
        }

        [Fact]
        public void ValidateAndSetAccessUrl_Should_Throw_When_AccessUrl_Is_Null_Or_Empty()
        {
            // Arrange
            var nullAccessUrl = (string?)null;
            var emptyAccessUrl = "";

            // Act & Assert
            Assert.Throws<DomainException>(() => StorageImage.CreatePublicImage(nullAccessUrl));
            Assert.Throws<DomainException>(() => StorageImage.CreatePublicImage(emptyAccessUrl));
        }

        [Fact]
        public void ValidateAndSetAccessUrl_Should_Throw_When_AccessUrl_Is_Invalid()
        {
            // Arrange
            var invalidAccessUrl = "invalid-url";

            // Act & Assert
            Assert.Throws<DomainException>(() => StorageImage.CreatePublicImage(invalidAccessUrl));
        }

        [Fact]
        public void ValidateAndSetAccessUrl_Should_Throw_When_AccessUrl_Has_Unsupported_Scheme()
        {
            // Arrange
            var unsupportedSchemeUrl = "ftp://example.com/image.jpg";

            // Act & Assert
            Assert.Throws<DomainException>(() => StorageImage.CreatePublicImage(unsupportedSchemeUrl));
        }

        [Fact]
        public void ValidateAndSetAccessUrl_Should_Set_Valid_Http_Or_Https_Url()
        {
            // Arrange
            var validHttpUrl = "http://example.com/image.jpg";
            var validHttpsUrl = "https://example.com/image.jpg";

            // Act
            var httpImage = StorageImage.CreatePublicImage(validHttpUrl);
            var httpsImage = StorageImage.CreatePublicImage(validHttpsUrl);

            // Assert
            Assert.Equal(new Uri(validHttpUrl), httpImage.AccessUrl);
            Assert.Equal(new Uri(validHttpsUrl), httpsImage.AccessUrl);
        }
    }
}