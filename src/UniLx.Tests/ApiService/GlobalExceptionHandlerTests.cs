using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text.Json;
using UniLx.ApiService.ExceptionHandlers;

namespace UniLx.Tests.ApiService
{
    public class GlobalExceptionHandlerTests
    {
        [Fact]
        public async Task TryHandleAsync_ShouldHandleExceptionAndLogError()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<GlobalExceptionHandler>>();
            var handler = new GlobalExceptionHandler(loggerMock.Object);

            var context = new DefaultHttpContext();
            var responseStream = new MemoryStream();
            context.Response.Body = responseStream;

            var exception = new Exception("Test server exception");

            // Act
            var result = await handler.TryHandleAsync(context, exception, CancellationToken.None);

            // Assert
            Assert.True(result); // Ensure the exception was handled
            Assert.Equal(StatusCodes.Status500InternalServerError, context.Response.StatusCode);
            Assert.Equal("application/problem+json", context.Response.ContentType);

            responseStream.Seek(0, SeekOrigin.Begin);
            var responseContent = await JsonSerializer.DeserializeAsync<ProblemDetails>(responseStream);
            Assert.NotNull(responseContent);
            Assert.Equal(StatusCodes.Status500InternalServerError, responseContent!.Status);
            Assert.Equal("An unexpected server error occurred.", responseContent.Title);

            // Verify logging by capturing Log method invocation
            loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString() == $"Exception occurred: {exception.Message}"),
                    exception,
                    (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()),
                Times.Once);
        }

    }
}
