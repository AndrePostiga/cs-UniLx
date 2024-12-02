using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using Moq.AutoMock;
using UniLx.Application.Behaviors;
using UniLx.Shared.Abstractions;

namespace UniLx.Tests.Application.Behaviors
{

    // Sample request class
    public class SampleRequest : IRequest<IResult>
    {
        public string Property { get; set; }
    }

    // Sample validator
    public class SampleRequestValidator : AbstractValidator<SampleRequest>
    {
        public SampleRequestValidator()
        {
            RuleFor(x => x.Property)
                .NotEmpty().WithMessage("Property cannot be empty.")
                .MinimumLength(5).WithMessage("Property must be at least 5 characters long.");
        }
    }

    public class CommandValidatorBehaviorTests
    {
        [Fact]
        public async Task Handle_NoValidators_ShouldCallNext()
        {
            // Arrange
            var mocker = new AutoMocker();
            mocker.Use<IEnumerable<IValidator<SampleRequest>>>(Enumerable.Empty<IValidator<SampleRequest>>());

            var nextMock = new Mock<RequestHandlerDelegate<IResult>>();
            nextMock.Setup(x => x()).ReturnsAsync(Results.Ok("Success"));

            var behavior = mocker.CreateInstance<CommandValidatorBehavior<SampleRequest, IResult>>();

            var request = new SampleRequest();

            // Act
            var result = await behavior.Handle(request, nextMock.Object, CancellationToken.None);

            // Assert
            nextMock.Verify(x => x(), Times.Once); // Verify next is called
            var okResult = Assert.IsType<Ok<string>>(result); // Verify result type
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal("Success", okResult.Value);
        }

        [Fact]
        public async Task Handle_WithValidationErrors_ShouldReturnBadRequest()
        {
            // Arrange
            var validationFailures = new List<ValidationFailure>
    {
        new ValidationFailure("Property", "Error message 1"),
        new ValidationFailure("Property", "Error message 2")
    };

            var validatorMock = new Mock<IValidator<SampleRequest>>();
            validatorMock.Setup(x => x.Validate(It.IsAny<ValidationContext<SampleRequest>>()))
                         .Returns(new ValidationResult(validationFailures));

            var mocker = new AutoMocker();
            mocker.Use<IEnumerable<IValidator<SampleRequest>>>(new[] { validatorMock.Object });

            var nextMock = new Mock<RequestHandlerDelegate<IResult>>();

            var behavior = mocker.CreateInstance<CommandValidatorBehavior<SampleRequest, IResult>>();

            var request = new SampleRequest();

            // Act
            var result = await behavior.Handle(request, nextMock.Object, CancellationToken.None);

            // Assert
            nextMock.Verify(x => x(), Times.Never); // Verify next is not called
            var badRequestResult = Assert.IsType<BadRequest<ValidationProblemDetails>>(result); // Match the actual type
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);

            var problemDetails = badRequestResult.Value;
            Assert.NotNull(problemDetails);
            Assert.Equal("One or more validation errors occurred.", problemDetails.Title);
            Assert.Contains("Property", problemDetails.Errors.Keys);
            Assert.Contains("Error message 1", problemDetails.Errors["Property"]);
            Assert.Contains("Error message 2", problemDetails.Errors["Property"]);
        }


        [Fact]
        public async Task Handle_WithValidRequest_ShouldCallNext()
        {
            // Arrange
            var validator = new SampleRequestValidator();

            var mocker = new AutoMocker();
            mocker.Use<IEnumerable<IValidator<SampleRequest>>>(new[] { validator });

            var nextMock = new Mock<RequestHandlerDelegate<IResult>>();
            nextMock.Setup(x => x()).ReturnsAsync(Results.Ok("Success"));

            var behavior = mocker.CreateInstance<CommandValidatorBehavior<SampleRequest, IResult>>();

            var request = new SampleRequest { Property = "Valid Value" };

            // Act
            var result = await behavior.Handle(request, nextMock.Object, CancellationToken.None);

            // Assert
            nextMock.Verify(x => x(), Times.Once); // Verify next is called
            var okResult = Assert.IsType<Ok<string>>(result); // Verify result type
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal("Success", okResult.Value);
        }
    }
}
