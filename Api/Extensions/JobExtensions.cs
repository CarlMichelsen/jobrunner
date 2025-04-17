using Application.Job.Options;
using Interface.Job;

namespace Api.Extensions;

public static class JobExtensions
{
    public static IHostApplicationBuilder RegisterGenericJob<TGenericJob>(this IHostApplicationBuilder builder)
        where TGenericJob : class, IGenericJob
    {
        var jobType = typeof(TGenericJob);
        var options = builder.Configuration
                          .GetRequiredSection(GenericJobsOptions.SectionName)
                          .Get<GenericJobsOptions>()
                      ?? throw new InvalidOperationException($"Section '{GenericJobsOptions.SectionName}' not found in configuration");

        var optionsItem = options.First(o => o.Key == jobType.Name);
        
        builder.Services
            .RegisterGenericJob(jobType, optionsItem.Value.Interval, optionsItem.Value.EnableJitter);
        
        return builder;
    }
    
    private static IServiceCollection RegisterGenericJob(this IServiceCollection services, Type tGenericJob, TimeSpan interval, bool enableJitter = false)
    {
        services
            .AddTransient(tGenericJob)
            .AddSingleton<IHostedService>(provider => 
                new FunctionBackgroundService(
                    provider.GetRequiredService<ILogger<FunctionBackgroundService>>(),
                    async cancellationToken => 
                    {
                        var job = (IGenericJob)provider.GetRequiredService(tGenericJob);
                        await job.Run(cancellationToken);
                    },
                    interval,
                    tGenericJob.Name ?? throw new Exception("Unable to get name of job"),
                    enableJitter));

        return services;
    }
}