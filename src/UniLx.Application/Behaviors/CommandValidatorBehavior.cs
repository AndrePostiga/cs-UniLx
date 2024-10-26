using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using UniLx.Shared.Abstractions;
namespace UniLx.Application.Behaviors;

public sealed class CommandValidatorBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<IResult>
    where TResponse : IResult  
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public CommandValidatorBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
            return await next();

        var context = new ValidationContext<TRequest>(request);

        var validationResults = _validators
            .Select(x => x.Validate(context))
            .SelectMany(x => x.Errors)
            .Where(x => x != null)
            .ToList();

        if (validationResults.Count != 0)
        {
            var errorsDict = validationResults
                .GroupBy(
                    x => x.PropertyName,
                    x => x.ErrorMessage,
                    (propertyName, errorMessages) => new
                    {
                        Key = propertyName,
                        Values = errorMessages.Distinct().ToArray()
                    })
                .ToDictionary(x => x.Key, x => x.Values);

            return (TResponse)Results.BadRequest(new ValidationProblemDetails(errorsDict));
            
        }

        return await next();
    }
}