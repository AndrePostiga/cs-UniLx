using Carter;

namespace UniLx.ApiService.Examples
{
    public class RouteExample : ICarterModule 
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/accounts", async () =>
            {

            })
            .RequireAuthorization();
        }
    }
}
