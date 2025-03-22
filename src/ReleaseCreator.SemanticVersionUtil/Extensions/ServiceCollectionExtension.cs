using Microsoft.Extensions.DependencyInjection;
using ReleaseCreator.SemanticVersionUtil.Builder;
using ReleaseCreator.SemanticVersionUtil.Incrementor;
using ReleaseCreator.SemanticVersionUtil.Parser;

namespace ReleaseCreator.SemanticVersionUtil.Extensions;

/// <summary>
/// Contains extension methods for adding semantic version util services to the DI container.
/// </summary>
public static class ServiceCollectionExtension
{
    /// <summary>
    /// Adds all services for the semantic version util.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    public static IServiceCollection AddSemanticVersionUtilServicesSingleton(this IServiceCollection services)
    {
        services.AddTransient<ISemanticVersionBuilder, SemanticVersionBuilder>()
        .AddSingleton<ISemanticVersionIncrementDirector, SemanticVersionIncrementDirector>()
        .AddSingleton<ISemanticVersionIncrementor, SemanticVersionIncrementor>()
        .AddSingleton<ISemanticVersionParser, SemanticVersionParser>();

        return services;
    }
}