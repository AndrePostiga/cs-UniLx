using UniLx.Shared.Abstractions;

namespace UniLx.Tests.Shared.Abstractions
{
    namespace UniLx.Tests.Shared.Abstractions
    {
        public class ValidationProblemDetailsTests
        {
            [Fact]
            public void Constructor_WithErrors_ShouldInitializeProperties()
            {
                // Arrange
                var errors = new Dictionary<string, string[]>
            {
                { "Field1", new[] { "Error1", "Error2" } },
                { "Field2", new[] { "Error3" } }
            };

                // Act
                var problemDetails = new ValidationProblemDetails(errors);

                // Assert
                Assert.NotNull(problemDetails);
                Assert.Equal((int)HttpStatusCode.BadRequest, problemDetails.Status);
                Assert.Equal("One or more validation errors occurred.", problemDetails.Title);
                Assert.Equal(errors, problemDetails.Errors);
            }

            [Fact]
            public void Constructor_WithError_ShouldInitializeProperties()
            {
                // Arrange
                var error = new Error(HttpStatusCode.BadRequest, "InvalidInput", "The input is invalid.");

                // Act
                var problemDetails = new ValidationProblemDetails(error);

                // Assert
                Assert.NotNull(problemDetails);
                Assert.Equal((int)HttpStatusCode.BadRequest, problemDetails.Status);
                Assert.Equal("One error occurred.", problemDetails.Title);
                Assert.NotNull(problemDetails.Errors);
                Assert.Single(problemDetails.Errors);
                Assert.True(problemDetails.Errors.ContainsKey("InvalidInput"));
                Assert.Contains("The input is invalid.", problemDetails.Errors["InvalidInput"]);
            }

            [Fact]
            public void Constructor_WithErrorAndNullDescription_ShouldNotInitializeErrors()
            {
                // Arrange
                var error = new Error(HttpStatusCode.BadRequest, "InvalidInput", null);

                // Act
                var problemDetails = new ValidationProblemDetails(error);

                // Assert
                Assert.NotNull(problemDetails);
                Assert.Equal((int)HttpStatusCode.BadRequest, problemDetails.Status);
                Assert.Equal("One error occurred.", problemDetails.Title);
                Assert.Null(problemDetails.Errors);
            }

            [Fact]
            public void Constructor_WithNullErrors_ShouldHandleGracefully()
            {
                // Act
                var problemDetails = new ValidationProblemDetails((Dictionary<string, string[]>?)null);

                // Assert
                Assert.NotNull(problemDetails);
                Assert.Equal((int)HttpStatusCode.BadRequest, problemDetails.Status);
                Assert.Equal("One or more validation errors occurred.", problemDetails.Title);
                Assert.Null(problemDetails.Errors);
            }
        }
    }

}
