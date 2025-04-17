namespace Application.Job.Options;

public class GenericJobOptionsItem
{
    public required TimeSpan Interval { get; init; }
    
    public required bool EnableJitter { get; init; }
}