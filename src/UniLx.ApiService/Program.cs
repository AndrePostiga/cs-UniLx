using UniLx.ApiService.ExceptionHandlers;
using UniLx.ApiService.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddExceptionHandler<DomainExceptionHandler>();
builder.Services.AddExceptionHandler<SupabaseExceptionHandler>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.AddApiConfiguration();
builder.AddRegisteredServices();


// APP
var app = builder.Build();
app.UseApiConfiguration();
app.Run();
