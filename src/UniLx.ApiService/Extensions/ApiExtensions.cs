using Carter;

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
