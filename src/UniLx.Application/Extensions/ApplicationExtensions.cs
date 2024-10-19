using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace UniLx.Application.Extensions
{
    public static class ApplicationExtensions
    {
        public static WebApplicationBuilder AddApplication(this WebApplicationBuilder builder)
        {
            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(typeof(ApplicationExtensions).Assembly);
            });

            return builder;
        }
    }
}
