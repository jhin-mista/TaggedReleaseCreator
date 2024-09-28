using Microsoft.Extensions.DependencyInjection;
using ReleaseCreator.SemanticVersionUtil.Builder;
using ReleaseCreator.SemanticVersionUtil.Incrementor;

namespace ReleaseCreator.SemanticVersionUtil.Extensions;

/// <summary>
/// Contains extension methods for 
/// </summary>
public static class ServiceCollectionExtension
{
    /// <summary>
    /// Adds all services for the version incrementor.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    public static IServiceCollection AddVersionIncrementorServicesSingleton(this IServiceCollection services)
    {
        services.AddSingleton<ISemanticVersionBuilder, SemanticVersionBuilder>();
        services.AddSingleton<ISemanticVersionIncrementDirector, SemanticVersionIncrementDirector>();
        services.AddSingleton<ISemanticVersionIncrementor, SemanticVersionIncrementor>();

        return services;
    }
}
