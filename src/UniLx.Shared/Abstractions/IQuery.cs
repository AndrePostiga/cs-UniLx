using MediatR;
using Microsoft.AspNetCore.Http;

namespace UniLx.Shared.Abstractions
{
    public interface IQuery<out TResponse> : IRequest<TResponse>
        where TResponse : IResult
    {
    }

    public interface IQueryInternal<out TResponse> : IRequest<TResponse>
        where TResponse : class?
    {
    }

    public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
        where TQuery : IQuery<TResponse>
        where TResponse : IResult
    {

    }

    public interface IQueryInternalHandler<TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
        where TQuery : IQueryInternal<TResponse>
        where TResponse : class?
    {

    }
}
