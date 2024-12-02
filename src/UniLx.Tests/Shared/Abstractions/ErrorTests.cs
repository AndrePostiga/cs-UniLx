using Microsoft.AspNetCore.Http;
using UniLx.Shared.Abstractions;

namespace UniLx.Tests.Shared.Abstractions
{
    public class ErrorTests
    {
        [Fact]
        public void Error_Creation_ShouldSetProperties()
        {
            // Arrange
            var statusCode = HttpStatusCode.BadRequest;
            var code = "SomeCode";
            var description = "Some description";

            // Act
            var error = new Error(statusCode, code, description);

            // Assert
            Assert.Equal(statusCode, error.StatusCode);
            Assert.Equal(code, error.Code);
            Assert.Equal(description, error.Description);
        }

        [Fact]
        public void Error_None_ShouldHaveDefaultValues()
        {
            // Act
            var noneError = Error.None;

            // Assert
            Assert.Equal(HttpStatusCode.OK, noneError.StatusCode);
            Assert.Equal(string.Empty, noneError.Code);
            Assert.Null(noneError.Description);
        }

        [Fact]
        public void ImplicitConversion_ToValidationProblemDetails_ShouldMapCorrectly()
        {
            // Arrange
            var error = new Error(HttpStatusCode.NotFound, "NotFound", "Resource not found");

            // Act
            ValidationProblemDetails problemDetails = error;

            // Assert
            Assert.Equal((int)HttpStatusCode.NotFound, problemDetails.Status);
            Assert.Equal("One error occurred.", problemDetails.Title);
            Assert.Single(problemDetails.Errors);
            Assert.True(problemDetails.Errors.ContainsKey("NotFound"));
            Assert.Contains("Resource not found", problemDetails.Errors["NotFound"]);
        }

        [Fact]
        public void ToBadRequest_ShouldReturnJsonResultWithCorrectProblemDetails()
        {
            // Arrange
            var error = new Error(HttpStatusCode.BadRequest, "InvalidInput", "The input provided is invalid.");

            // Act
            var result = error.ToBadRequest();

            // Assert
            Assert.NotNull(result);

            // Verify the result is of the expected generic JSON HTTP result type
            var jsonResult = Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.JsonHttpResult<ValidationProblemDetails>>(result);

            // Validate the status code
            Assert.Equal(StatusCodes.Status400BadRequest, jsonResult.StatusCode);

            // Validate the content of the problem details
            var problemDetails = jsonResult.Value;
            Assert.NotNull(problemDetails);
            Assert.Equal(StatusCodes.Status400BadRequest, problemDetails.Status);
            Assert.Equal("One error occurred.", problemDetails.Title);
            Assert.Single(problemDetails.Errors);
            Assert.True(problemDetails.Errors.ContainsKey("InvalidInput"));
            Assert.Contains("The input provided is invalid.", problemDetails.Errors["InvalidInput"]);
        }

    }

}
