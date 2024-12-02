using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text.Json;
using UniLx.ApiService.ExceptionHandlers;
using UniLx.Domain.Exceptions;

namespace UniLx.Tests.ApiService
{
    public class DomainExceptionHandlerTests
    {
        [Fact]
        public async Task TryHandleAsync_DomainException_ShouldHandleException()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<DomainExceptionHandler>>();
            var handler = new DomainExceptionHandler(loggerMock.Object);

            var context = new DefaultHttpContext();
            var responseStream = new MemoryStream();
            context.Response.Body = responseStream;

            var exception = new DomainException("Test domain exception");

            // Act
            var result = await handler.TryHandleAsync(context, exception, CancellationToken.None);

            // Assert
            Assert.True(result); // Ensure the exception was handled
            Assert.Equal((int)HttpStatusCode.PreconditionFailed, context.Response.StatusCode);

            responseStream.Seek(0, SeekOrigin.Begin);
            var responseContent = await JsonSerializer.DeserializeAsync<ProblemDetails>(responseStream);
            Assert.NotNull(responseContent);
            Assert.Equal("A domain exception occurred", responseContent!.Title);
            Assert.Equal("Test domain exception", responseContent.Detail);
            Assert.Equal(nameof(DomainException), responseContent.Type);
        }

        [Fact]
        public async Task TryHandleAsync_NonDomainException_ShouldNotHandleException()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<DomainExceptionHandler>>();
            var handler = new DomainExceptionHandler(loggerMock.Object);

            var context = new DefaultHttpContext();
            var exception = new Exception("Test non-domain exception");

            // Act
            var result = await handler.TryHandleAsync(context, exception, CancellationToken.None);

            // Assert
            Assert.False(result); // Ensure the exception was not handled
            Assert.Equal(StatusCodes.Status200OK, context.Response.StatusCode); // Default status code
        }
    }
}
