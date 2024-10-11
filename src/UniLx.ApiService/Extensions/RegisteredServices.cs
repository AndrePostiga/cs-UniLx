namespace UniLx.ApiService.Extensions
{
    public static class RegisteredServices
    {
        public static WebApplicationBuilder AddRegisteredServices(this WebApplicationBuilder builder)
        {

            builder.Services.AddHttpClient("GoogleCertsClient", client =>
            {
                client.BaseAddress = new Uri("https://www.googleapis.com/oauth2/v3");
            });

            return builder;
        }
    }
}
