using Marten;
using Microsoft.AspNetCore.Mvc;
using UniLx.ApiService.Extensions;
using UniLx.Domain.Entities.AccountAgg;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();


// Add services to the container.
builder.Services.AddProblemDetails();
builder.AddDatabase();
builder.AddApiConfiguration();
builder.AddRegisteredServices();


// APP

var app = builder.Build();

#region webforecast

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", ([FromServices]IDocumentSession session) =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();

    var acc = new Account(
        name: "Andre Postiga",
        email: new UniLx.Domain.Entities.AccountAgg.ValueObj.Email("andre@andre.com"),
        Cpf: new UniLx.Domain.Entities.AccountAgg.ValueObj.CPF("15792539707"),
        image: new UniLx.Domain.Entities.AdvertisementAgg.ValueObj.Image(new Uri("https://andrepostiga.github.io/images/service/telecommunications.png"), "png", 1, 1, 1, "algum nome", "_self"));

   
    
    session.Store(acc);
    session.SaveChanges();
    return forecast;
});


#endregion

app.UseApiConfiguration();

app.Run();


#region webforecast
record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
#endregion