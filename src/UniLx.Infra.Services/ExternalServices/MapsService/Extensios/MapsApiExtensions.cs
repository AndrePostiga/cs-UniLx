using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UniLx.Domain.Services;
using UniLx.Infra.Services.ExternalServices.MapsService.Options;

namespace UniLx.Infra.Services.ExternalServices.MapsService.Extensios
{
    public static class MapsApiExtensions
    {
        public static WebApplicationBuilder AddMapsApi(this WebApplicationBuilder builder)
        {

            var mapsApiOptions = builder.Configuration.GetSection(MapsApiOptions.Section).Get<MapsApiOptions>();
            builder.Services.Configure<MapsApiOptions>(builder.Configuration.GetSection(MapsApiOptions.Section));

            builder.Services.AddHttpClient<IMapsService, MapsService>(client =>
            {
                client.BaseAddress = new Uri(mapsApiOptions!.Url!);
                client.Timeout = TimeSpan.FromSeconds(mapsApiOptions.TimeoutInSeconds);
            });

            return builder;
        }
    }
}
