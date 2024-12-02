using UniLx.Shared.Abstractions;

namespace UniLx.Tests.Shared.Abstractions
{
    using System;
    using Xunit;

    namespace UniLx.Tests.Shared.Abstractions
    {
        public class ServiceResultTests
        {
            private class TestContent
            {
                public string Data { get; set; } = "Test";
            }

            [Fact]
            public void Success_ShouldCreateSuccessfulResult()
            {
                // Arrange
                var content = new TestContent();

                // Act
                var result = ServiceResult<TestContent>.Success(content);

                // Assert
                Assert.NotNull(result);
                Assert.True(result.IsSuccess);
                Assert.False(result.IsError);
                Assert.Equal(content, result.Content);
                Assert.Equal(Error.None, result.Error);
            }

            [Fact]
            public void Failure_ShouldCreateFailureResult()
            {
                // Arrange
                var error = new Error(System.Net.HttpStatusCode.BadRequest, "InvalidInput", "The input is invalid.");

                // Act
                var result = ServiceResult<TestContent>.Failure(error);

                // Assert
                Assert.NotNull(result);
                Assert.False(result.IsSuccess);
                Assert.True(result.IsError);
                Assert.Null(result.Content);
                Assert.Equal(error, result.Error);
            }

            [Fact]
            public void Success_Generic_ShouldCreateSuccessfulResult()
            {
                // Arrange
                var content = new TestContent();

                // Act
                var result = ServiceResult.Success(content);

                // Assert
                Assert.NotNull(result);
                Assert.True(result.IsSuccess);
                Assert.False(result.IsError);
                Assert.Equal(content, result.Content);
                Assert.Equal(Error.None, result.Error);
            }
        }
    }
}
