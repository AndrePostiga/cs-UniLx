using MediatR;
using Microsoft.AspNetCore.Http;

namespace UniLx.Shared.Abstractions
{
    public interface IQuery<TResponse> : IRequest<TResponse>
        where TResponse : IResult
    {
    }

    public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
        where TQuery : IQuery<TResponse>
        where TResponse : IResult
    {

    }
}
