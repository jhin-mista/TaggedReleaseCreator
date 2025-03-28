﻿using Microsoft.Extensions.DependencyInjection;
using ReleaseCreator.Git.ShellExecution;
using ReleaseCreator.Git.Tag;

namespace ReleaseCreator.Git.Extensions;

/// <summary>
/// Contains extension methods for adding git services to the DI container.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds services required for using git.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    public static IServiceCollection AddGitServicesSingleton(this IServiceCollection services)
    {
        services.AddLogging()
            .AddSingleton<ITagRetriever, TagRetriever>()
            .AddSingleton<IPowerShellExecutor, PowerShellExecutor>();

        return services;
    }
}