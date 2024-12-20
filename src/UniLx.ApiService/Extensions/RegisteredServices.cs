﻿using System.Diagnostics.CodeAnalysis;
using UniLx.Application.Extensions;
using UniLx.Domain.Services;
using UniLx.Infra.Data.ServiceExtensions;
using UniLx.Infra.Services.ExternalServices.MapsService.Extensios;
using UniLx.Infra.Services.ExternalServices.Supabase;

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
            builder.AddSupabase();
            builder.AddStorage();

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
