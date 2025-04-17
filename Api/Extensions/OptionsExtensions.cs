using Interface.Options;

namespace Api.Extensions;

public static class OptionsExtensions
{
    public static IHostApplicationBuilder AddValidatedOptions<TOptions>(this IHostApplicationBuilder builder)
        where TOptions : class, IOptionsImpl
    {
        builder.Services
            .AddOptions<TOptions>()
            .Bind(builder.Configuration.GetSection(TOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return builder;
    }
}