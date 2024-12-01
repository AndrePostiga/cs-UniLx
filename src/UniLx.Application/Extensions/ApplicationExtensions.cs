using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using UniLx.Application.Behaviors;

namespace UniLx.Application.Extensions
{
    public static class ApplicationExtensions
    {
        public static WebApplicationBuilder AddApplication(this WebApplicationBuilder builder)
        {
            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(typeof(ApplicationExtensions).Assembly);
                cfg.AddOpenBehavior(typeof(CommandValidatorBehavior<,>));
            });

            builder.Services.AddValidatorsFromAssembly(typeof(ApplicationExtensions).Assembly, includeInternalTypes:true);          

            return builder;
        }
    }
}
