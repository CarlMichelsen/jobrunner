using Api;
using Api.Middleware;
using Application;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddApplicationDependencies();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseMiddleware<TraceIdMiddleware>();

app.UseSerilogRequestLogging();

app.Services.GetRequiredService<ILogger<Program>>()
    .LogInformation(
        "{ApplicationName} service has started",
        ApplicationConstants.Name);

app.Run();
