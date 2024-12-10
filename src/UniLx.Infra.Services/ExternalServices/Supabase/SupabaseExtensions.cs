using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Supabase;

namespace UniLx.Infra.Services.ExternalServices.Supabase
{
    public static class SupabaseExtensions
    {
        public static WebApplicationBuilder AddSupabase(this WebApplicationBuilder builder)
        {

            var supabaseSettings = builder.Configuration.GetSection(SupabaseClientOptions.Section).Get<SupabaseClientOptions>();
            builder.Services.Configure<SupabaseClientOptions>(builder.Configuration.GetSection(SupabaseClientOptions.Section));

            builder.Services.AddScoped(_ => new Client(
                supabaseSettings!.Url!,
                supabaseSettings!.PrivateKey!,
                new SupabaseOptions
                {
                    AutoRefreshToken = true,
                    AutoConnectRealtime = true,
                }));

            return builder;
        }
    }
}
