namespace Api;

public class FunctionBackgroundService(
    ILogger<FunctionBackgroundService> logger,
    Func<CancellationToken, Task> action,
    TimeSpan interval,
    string jobName) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var randomStartTime = (double)Random.Shared.Next() / int.MaxValue * 10;

        logger.LogInformation(
            "Initiating {JobName} job in {Seconds}...",
            jobName,
            (int)Math.Round(randomStartTime));
        await Task.Delay(TimeSpan.FromSeconds(randomStartTime), stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                logger.LogInformation("Running {JobName} job", jobName);
                await action(stoppingToken);
                logger.LogInformation(
                    "Job {JobName} job completed, running again after timespan {Interval}",
                    jobName,
                    interval);
            }
            catch (OperationCanceledException)
            {
                logger.LogWarning(
                    "Job {JobName} was cancelled gracefully",
                    jobName);
            }
            catch (Exception e)
            {
                logger.LogError(
                    e, 
                    "An exception was thrown while running job {JobName}",
                    jobName);
            }

            await Task.Delay(interval, stoppingToken);
        }
    }
}