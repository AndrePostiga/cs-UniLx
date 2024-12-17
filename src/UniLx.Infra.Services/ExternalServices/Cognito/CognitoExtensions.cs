﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using System.Diagnostics.CodeAnalysis;

namespace UniLx.Infra.Services.ExternalServices.Cognito
{
    [ExcludeFromCodeCoverage]
    public static class CognitoExtensions
    {
        public static WebApplicationBuilder AddCognitoService(this WebApplicationBuilder builder)
        {
            var configuration = builder.Configuration;
            var cognitoOptions = configuration.GetSection(CognitoOptions.Section).Get<CognitoOptions>();

            builder.Services.AddRefitClient<ICognitoService>()
                            .ConfigureHttpClient(client =>
                            {
                                client.BaseAddress = new Uri(cognitoOptions!.Domain!);
                            });


            return builder;
        }
    }
}
