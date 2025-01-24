using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace UniLx.Domain.Extensions
{
    public static class DomainExtensions
    {
        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            return services;
        }
    }
}
