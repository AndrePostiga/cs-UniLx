using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using UniLx.Application.Behaviors;

namespace UniLx.Application.Extensions
{
    [ExcludeFromCodeCoverage]
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
