using System.Reflection;
using UniLx.ApiService.ExceptionHandlers;
using UniLx.ApiService.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables("UniLx_");

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opts =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    opts.IncludeXmlComments(xmlPath);
});

builder.Services.AddExceptionHandler<DomainExceptionHandler>();
builder.Services.AddExceptionHandler<SupabaseExceptionHandler>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

builder.AddApiConfiguration();
builder.AddRegisteredServices();
builder.AddCustomAuthenticationAndAuthorization();

// APP
var app = builder.Build();
app.UseApiConfiguration();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

await app.RunAsync();
