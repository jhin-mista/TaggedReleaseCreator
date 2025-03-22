using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ReleaseCreator.Client.Extensions;
using ReleaseCreator.Git.Extensions;
using ReleaseCreator.SemanticVersionUtil.Extensions;

namespace ReleaseCreator.Client.Bootstrap;

/// <summary>
/// Represents the bootstrapping logic for the dependency injection container.
/// </summary>
public static class ContainerBootstrapper
{
    /// <summary>
    /// Builds up the dependency injection container and returns the corresponding <see cref="ServiceProvider"/>.
    /// </summary>
    /// <param name="accessToken">The access token.</param>
    public static ServiceProvider BuildUp(string accessToken)
    {
        var services = new ServiceCollection();

        services.AddLogging(AddConsoleLoggerWithLogLevelSetByEnvironmentVariable)
            .AddVersionIncrementorServicesSingleton()
            .AddGitServicesSingleton()
            .AddReleaseCreatorClientServicesSingleton(accessToken);

        return services.BuildServiceProvider();
    }

    private static void AddConsoleLoggerWithLogLevelSetByEnvironmentVariable(ILoggingBuilder builder)
    {
        var config = new ConfigurationBuilder().AddEnvironmentVariables().Build();

        builder.AddConsole()
            .AddConfiguration(config.GetSection("Logging"));
    }
}