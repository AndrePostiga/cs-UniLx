using Carter;
using Marten;
using Microsoft.AspNetCore.Mvc;

namespace UniLx.ApiService.Controllers.Example
{
    record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
    {
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }

    public class WeatherForecastController : ICarterModule
    {


        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/weatherforecast", ([FromServices] IDocumentSession session) =>
            {
                var summaries = new[]
                {
                    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
                };

                var forecast = Enumerable.Range(1, 5).Select(index =>
                    new WeatherForecast
                    (
                        DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                        Random.Shared.Next(-20, 55),
                        summaries[Random.Shared.Next(summaries.Length)]
                    ))
                    .ToArray();

                //var acc = new Account(
                //    name: "Andre Postiga",
                //    email: new UniLx.Domain.Entities.AccountAgg.ValueObj.Email("andre@andre.com"),
                //    Cpf: new UniLx.Domain.Entities.AccountAgg.ValueObj.CPF("15792539707"),
                //    image: new UniLx.Domain.Entities.Seedwork.ValueObj.Image(new Uri("https://andrepostiga.github.io/images/service/telecommunications.png"), "png", 1, 1, 1, "algum nome", "_self"));



                //session.Store(acc);
                //session.SaveChanges();
                return forecast;
            });
        }

    }
}
