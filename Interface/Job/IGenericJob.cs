namespace Interface.Job;

public interface IGenericJob
{
    Task Run(CancellationToken stoppingToken);
}