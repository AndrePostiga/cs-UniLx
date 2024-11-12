using UniLx.Application.Extensions;
using UniLx.Infra.Data.ServiceExtensions;
using UniLx.Infra.Services.ExternalServices.Supabase;

namespace UniLx.ApiService.Extensions
{
    public static class RegisteredServices
    {
        public static WebApplicationBuilder AddRegisteredServices(this WebApplicationBuilder builder)
        {

            //builder.Services.AddHttpClient("GoogleCertsClient", client =>
            //{
            //    client.BaseAddress = new Uri("https://www.googleapis.com/oauth2/v3");
            //});

            builder.AddApplication();
            builder.AddDatabase();
            builder.AddSupabase();
            builder.AddStorage();


            return builder;
        }
    }
}
