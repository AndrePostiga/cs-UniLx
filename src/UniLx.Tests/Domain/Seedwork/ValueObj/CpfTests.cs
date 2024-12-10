using UniLx.Domain.Entities.Seedwork.ValueObj;
using UniLx.Domain.Exceptions;

namespace UniLx.Tests.Domain.Seedwork.ValueObj
{
    public class CPFTests
    {
        [Theory]
        [InlineData("12345678909")] // Example of a valid CPF
        [InlineData("11144477735")]
        [InlineData("93541134780")]
        public void CPF_ValidCPF_ShouldCreateObject(string validCpf)
        {
            // Act
            var cpf = new CPF(validCpf);

            // Assert
            Assert.Equal(validCpf, cpf.ToString());
        }

        [Theory]
        [InlineData("12345678900")] // Invalid digit
        [InlineData("11111111111")] // All digits the same
        [InlineData("123")]         // Too short
        [InlineData("")]            // Empty string
        public void CPF_InvalidCPF_ShouldThrowException(string invalidCpf)
        {
            // Act & Assert
            var exception = Assert.Throws<DomainException>(() => new CPF(invalidCpf));
            Assert.Equal("Invalid CPF.", exception.Message);
        }

        [Fact]
        public void CPF_WithNonNumericCharacters_ShouldThrowException()
        {
            // Arrange
            string invalidCpf = "000.000.000-00";

            // Act & Assert
            var exception = Assert.Throws<DomainException>(() => new CPF(invalidCpf));
            Assert.Equal("Invalid CPF.", exception.Message);
        }

        [Fact]
        public void CPF_ToString_ShouldReturnCorrectValue()
        {
            // Arrange
            string validCpf = "93541134780";
            var cpf = new CPF(validCpf);

            // Act
            var result = cpf.ToString();

            // Assert
            Assert.Equal(validCpf, result);
        }

        [Fact]
        public void CPF_ValidationPerformance_ShouldHandleLargeInputQuickly()
        {
            // Arrange
            string veryLongCpf = new string('1', 10000);

            // Act
            var isValid = CPF.IsValid(veryLongCpf);

            // Assert
            Assert.False(isValid);
        }
    }
}