using Api.Extensions;
using Api.Logging;
using Api.Middleware;
using Application.HttpClient;
using Application.HttpClient.Options;
using Browser;
using Browser.Options;
using Interface.Discord;
using Job.RoyalRun;
using Serilog;

namespace Api;

public static class Dependencies
{
    public static void AddApplicationDependencies(this WebApplicationBuilder builder)
    {
        // Configuration
        builder.Configuration.AddJsonFile(
            "secrets.json",
            optional: builder.Environment.IsDevelopment(),
            reloadOnChange: false);
        
        builder
            .AddValidatedOptions<BrowserOptions>()
            .AddValidatedOptions<DiscordClientOptions>();
        
        // Implementations
        builder.Services
            .AddOpenApi()
            .AddHttpContextAccessor()
            .AddMemoryCache()
            .AddSingleton<BrowserHandler>()
            .AddTransient<RoyalRunJob>();
        
        // HttpClients
        builder.Services
            .AddHttpClient<IDiscordClient, DiscordClient>();
        
        // Middleware
        builder.Services
            .AddTransient<TraceIdEnricher>()
            .AddScoped<TraceIdMiddleware>();
        
        // Serilog
        builder.Host.UseSerilog((context, sp, configuration) =>
        {
            configuration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(sp)
                .Enrich.With(sp.GetRequiredService<TraceIdEnricher>())
                .Enrich.WithProperty("Application", "JobRunner")
                .Enrich.WithProperty("Environment", GetEnvironmentName(builder));
        });
        builder.Services.AddSingleton<TraceIdEnricher>();
        
        // Jobs
        builder.Services
            .RegisterGenericJob<RoyalRunJob>("RoyalRun", TimeSpan.FromMinutes(10));
    }
    
    private static string GetEnvironmentName(WebApplicationBuilder builder) =>
        builder.Environment.IsProduction() ? "Production" : "Development";
}