using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.CodeAnalysis;
using UniLx.ApiService.Authorization;
using UniLx.Infra.Services.ExternalServices.Cognito;

namespace UniLx.ApiService.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class JwtBearerExtensions
    {
        public static WebApplicationBuilder AddCustomAuthenticationAndAuthorization(this WebApplicationBuilder builder)
        {
            builder.Services.AddAuthorization();

            var configuration = builder.Configuration;
            var cognitoOptions = configuration.GetSection(CognitoOptions.Section).Get<CognitoOptions>();
            builder.Services.Configure<CognitoOptions>(builder.Configuration.GetSection(CognitoOptions.Section));

            builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearerWithDecorator(JwtBearerDefaults.AuthenticationScheme, x =>
                {
                    x.Authority = cognitoOptions!.Authority!;
                    x.MetadataAddress = cognitoOptions!.MetadataAddress!;
                    x.IncludeErrorDetails = true;
                    x.RequireHttpsMetadata = false;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        RoleClaimType = cognitoOptions!.RoleClaimType!,
                        ValidateLifetime = true,
                        ValidateIssuer = true,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = cognitoOptions!.Authority!
                    };
                });

            return builder;
        }

        public static AuthenticationBuilder AddJwtBearerWithDecorator(
        this AuthenticationBuilder builder,
        string authenticationScheme,
        Action<JwtBearerOptions> configureOptions)
        {

            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<JwtBearerOptions>, JwtBearerPostConfigureOptions>());
            builder.Services.AddScoped<IAuthenticationHandler, JwtAuthHandlerDecorator>();
            return builder.AddScheme<JwtBearerOptions, JwtAuthHandlerDecorator>(authenticationScheme, configureOptions);
        }
    }
}
