using Carter;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;

namespace UniLx.ApiService.Extensions
{
    public static class ApiExtensions
    {
        public static WebApplicationBuilder AddApiConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.AddCarter();
            return builder;
        }

        public static WebApplication UseApiConfiguration(this WebApplication webApplication)
        {
            webApplication
                .MapDefaultEndpoints()
                .MapCarter();

            webApplication
                .UseExceptionHandler();

            return webApplication;
        }
    }
}
