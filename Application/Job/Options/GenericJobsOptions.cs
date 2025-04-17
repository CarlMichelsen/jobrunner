using Interface.Options;

namespace Application.Job.Options;

public class GenericJobsOptions : Dictionary<string, GenericJobOptionsItem>, IOptionsImpl
{
    public static string SectionName => "GenericJobs";
}