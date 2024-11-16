using Microsoft.Extensions.DependencyInjection;
using ReleaseCreator.Git.ShellExecution;
using ReleaseCreator.Git.Tag;

namespace ReleaseCreator.Git.Extensions;

/// <summary>
/// Extensions methods for adding git services to the DI container.
/// </summary>
public static class GitServiceCollectionExtensions
{
    /// <summary>
    /// Adds services required for using git.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    public static IServiceCollection AddGitServicesSingleton(this IServiceCollection services)
    {
        services.AddSingleton<ITagRetriever, TagRetriever>();
        services.AddSingleton<IPowerShellExecutor, PowerShellExecutor>();

        return services;
    }
}