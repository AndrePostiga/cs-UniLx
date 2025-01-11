using System.Diagnostics.CodeAnalysis;
using UniLx.Application.Extensions;
using UniLx.Domain.Services;
using UniLx.Infra.Data.Bus;
using UniLx.Infra.Data.Database;
using UniLx.Infra.Data.Storage;
using UniLx.Infra.Services.ExternalServices.Cognito;
using UniLx.Infra.Services.ExternalServices.MapsService.Extensios;

namespace UniLx.ApiService.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class RegisteredServices
    {
        public static WebApplicationBuilder AddRegisteredServices(this WebApplicationBuilder builder)
        {
            builder.AddApplication();
            builder.AddDomainServices();
            builder.AddExternalServices();
            builder.AddDatabase();
            builder.AddStorage();
            builder.AddCognitoService();
            builder.AddKafkaBus();

            return builder;
        }

        private static WebApplicationBuilder AddDomainServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<ICreateAdvertisementDomainService, CreateAdvertisementDomainService>();
            return builder;
        }

        private static WebApplicationBuilder AddExternalServices(this WebApplicationBuilder builder)
        {
            builder.AddMapsApi();
            return builder;
        }
    }
}
