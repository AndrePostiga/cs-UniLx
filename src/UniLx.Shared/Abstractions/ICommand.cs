using MediatR;
using Microsoft.AspNetCore.Http;

namespace UniLx.Shared.Abstractions
{
    // Command with no return value (void command)
    public interface ICommand : IRequest<Unit> // Unit is used to represent void in MediatR
    {
    }

    // Command with a return value of type TResponse
    public interface ICommand<TResponse> : IRequest<TResponse>
        where TResponse : IResult
    {
    }

    // Command handler for commands that return nothing (void command handler)
    public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, Unit>
        where TCommand : ICommand
    {

    }

    // Command handler for commands that return a result of type TResponse
    public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
        where TResponse : IResult
    {
    }
}
