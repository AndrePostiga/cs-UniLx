namespace UniLx.Shared.Tests
{
    public class ExtensionsHelperTests
    {
        [Fact]
        public void OnlyNumbers_ShouldReturnOnlyDigits()
        {
            // Arrange
            var input = "abc123!@#456";
            var expected = "123456";

            // Act
            var result = input.OnlyNumbers();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void OnlyLetters_ShouldReturnOnlyLetters()
        {
            // Arrange
            var input = "abc123!@#DEF";
            var expected = "abcDEF";

            // Act
            var result = input.OnlyLetters();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void OnlyLetterOrNumbers_ShouldReturnOnlyLettersAndDigits()
        {
            // Arrange
            var input = "abc123!@#DEF456";
            var expected = "abc123DEF456";

            // Act
            var result = input.OnlyLetterOrNumbers();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void OnlyLetterOrNumbersWithSpaces_ShouldReturnLettersDigitsAndSpaces()
        {
            // Arrange
            var input = "abc 123!@# DEF 456";
            var expected = "abc 123 DEF 456";

            // Act
            var result = input.OnlyLetterOrNumbersWithSpaces();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void WithoutSpaces_ShouldRemoveAllSpaces()
        {
            // Arrange
            var input = "abc 123 DEF 456";
            var expected = "abc123DEF456";

            // Act
            var result = input.WithoutSpaces();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ToTitleCase_ShouldCapitalizeFirstLetter()
        {
            // Arrange
            var input = "example";
            var expected = "Example";

            // Act
            var result = input.ToTitleCase();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void WithTrimmedSpaces_StringInput_ShouldTrimSpacesBetweenWords()
        {
            // Arrange
            var input = "  This   is   a  test  ";
            var expected = "This is a test";

            // Act
            var result = input.WithTrimmedSpaces();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void WithTrimmedSpaces_ListInput_ShouldTrimSpacesInEachElement()
        {
            // Arrange
            var input = new List<string> { "  hello  ", "   world   ", "  test   " };
            var expected = new List<string> { "hello", "world", "test" };

            // Act
            var result = input.WithTrimmedSpaces();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void NullOrEmptyInputs_ShouldReturnOriginalInput()
        {
            // Arrange
            string? nullInput = null;
            var emptyInput = string.Empty;

            // Act & Assert
            Assert.Null(nullInput.OnlyNumbers());
            Assert.Null(nullInput.OnlyLetters());
            Assert.Null(nullInput.OnlyLetterOrNumbers());
            Assert.Null(nullInput.OnlyLetterOrNumbersWithSpaces());
            Assert.Null(nullInput.WithoutSpaces());
            Assert.Null(nullInput.ToTitleCase());
            Assert.Null(nullInput.WithTrimmedSpaces());

            Assert.Equal(string.Empty, emptyInput.OnlyNumbers());
            Assert.Equal(string.Empty, emptyInput.OnlyLetters());
            Assert.Equal(string.Empty, emptyInput.OnlyLetterOrNumbers());
            Assert.Equal(string.Empty, emptyInput.OnlyLetterOrNumbersWithSpaces());
            Assert.Equal(string.Empty, emptyInput.WithoutSpaces());
            Assert.Equal(string.Empty, emptyInput.ToTitleCase());
            Assert.Equal(string.Empty, emptyInput.WithTrimmedSpaces());
        }
    }
}


