using Interface.Job;

namespace Api.Extensions;

public static class JobExtensions
{
    public static IServiceCollection RegisterGenericJob<TGenericJob>(this IServiceCollection services, string jobName, TimeSpan interval)
        where TGenericJob : class, IGenericJob
    {
        services.AddSingleton<IHostedService>(provider => 
            new FunctionBackgroundService(
                provider.GetRequiredService<ILogger<FunctionBackgroundService>>(),
                async cancellationToken => 
                {
                    var job = (TGenericJob)provider.GetRequiredService(typeof(TGenericJob));
                    await job.Run(cancellationToken);
                },
                interval,
                jobName));

        return services;
    }
}