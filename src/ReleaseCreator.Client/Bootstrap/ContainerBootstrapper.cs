using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Octokit;
using Octokit.Internal;
using ReleaseCreator.Client.ReleaseCreation;
using ReleaseCreator.Client.ReleaseCreation.GitHub;
using ReleaseCreator.Client.Util;
using ReleaseCreator.Client.VersionCalculation;
using ReleaseCreator.Git.Extensions;
using ReleaseCreator.SemanticVersionUtil.Extensions;

namespace ReleaseCreator.Client.Bootstrap;

/// <summary>
/// Represents the bootstrapping logic for the dependency injection container.
/// </summary>
public static class ContainerBootstrapper
{
    /// <summary>
    /// Gets or sets the factory for creating an <see cref="IReleasesClient"/>.
    /// </summary>
    public static Func<string, IReleasesClient> ReleasesClientFactory { get; set; } = GetReleasesClient;

    /// <summary>
    /// Builds up the dependency injection container and returns the corresponding <see cref="ServiceProvider"/>.
    /// </summary>
    /// <param name="accessToken">The access token.</param>
    public static ServiceProvider BuildUp(string accessToken)
    {
        var client = ReleasesClientFactory(accessToken);
        var services = new ServiceCollection();

        services.AddLogging(AddConsoleLoggerWithLogLevelSetByEnvironmentVariable)
            .AddVersionIncrementorServicesSingleton()
            .AddGitServicesSingleton()
            .AddSingleton<IEnvironmentService, EnvironmentService>()
            .AddSingleton<IFileService, FileService>()
            .AddSingleton<INextVersionCalculator, NextVersionCalculator>()
            .AddSingleton(_ => client)
            .AddSingleton<IReleaseCreator, GitHubReleaseCreator>()
            .AddSingleton<Application>();

        return services.BuildServiceProvider();
    }

    private static ReleasesClient GetReleasesClient(string accessToken)
    {
        var userAgent = new ProductHeaderValue("ReleaseCreator");
        var credentials = new Credentials(accessToken);
        var credentialStore = new InMemoryCredentialStore(credentials);
        var connection = new Connection(userAgent, credentialStore);
        var apiConnection = new ApiConnection(connection);

        return new ReleasesClient(apiConnection);
    }

    private static void AddConsoleLoggerWithLogLevelSetByEnvironmentVariable(ILoggingBuilder builder)
    {
        var config = new ConfigurationBuilder().AddEnvironmentVariables().Build();

        builder.AddConsole()
            .AddConfiguration(config.GetSection("Logging"));
    }
}