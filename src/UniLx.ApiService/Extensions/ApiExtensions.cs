using Carter;
using Microsoft.AspNetCore.Http.Json;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using UniLx.Infra.Services.ChatHubs;
using UniLx.Shared.Abstractions;
using UniLx.Shared.Converters;

namespace UniLx.ApiService.Extensions
{
    [ExcludeFromCodeCoverage]
    [SuppressMessage("Security", "S5122", Justification = "This API is public and does not expose sensitive data.")]
    public static class ApiExtensions
    {
        public static WebApplicationBuilder AddApiConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllAllowed", 
                    services => services
                            .WithOrigins("http://localhost:5173", "http://10.255.255.254:5173", "http://127.0.0.1:5173", "http://172.17.240.1:5173")
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials()
                            .WithExposedHeaders("Location"));
            });

            builder.Services.Configure<JsonOptions>(options => 
            {
                options.SerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.SnakeCaseLower;
                options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                options.SerializerOptions.Converters.Add(new FloatConverter(2));

            });
            
            builder.Services.AddCarter();
            builder.Services.AddSignalR();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<IRequestContext, RequestContext.RequestContext>();
            return builder;
        }

        public static WebApplication UseApiConfiguration(this WebApplication webApplication)
        {
            webApplication.UseCors("AllAllowed");

            webApplication.UseAuthentication();
            webApplication.UseAuthorization();

            webApplication
                .MapDefaultEndpoints()
                .MapCarter();

            webApplication
                .MapHub<AdvertisementMessageChatHub>("/chat/advertisement")
                .RequireAuthorization(new AllowedGroups(Groups.User));

            webApplication.UseWebSockets();

            webApplication
                .UseExceptionHandler();

            return webApplication;
        }
    }
}
