using Carter;

namespace UniLx.ApiService.Controllers.Accounts
{
    public class PublicControllers : ICarterModule
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
