using Carter;
using Microsoft.AspNetCore.Http.Json;
using System.Text.Json.Serialization;
using UniLx.Shared.Converters;

namespace UniLx.ApiService.Extensions
{
    public static class ApiExtensions
    {
        public static WebApplicationBuilder AddApiConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllAllowed",
                    services =>
                        services
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .WithExposedHeaders("Location"));
            });

            builder.Services.Configure<JsonOptions>(options => 
            {
                options.SerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.SnakeCaseLower;
                options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                options.SerializerOptions.Converters.Add(new FloatConverter(2));

            });

            builder.Services.AddCarter();
            return builder;
        }

        public static WebApplication UseApiConfiguration(this WebApplication webApplication)
        {
            webApplication.UseCors("AllAllowed");

            webApplication
                .MapDefaultEndpoints()
                .MapCarter();

            webApplication
                .UseExceptionHandler();

            return webApplication;
        }
    }
}
