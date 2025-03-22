using Microsoft.Extensions.DependencyInjection;
using Octokit;
using Octokit.Internal;
using ReleaseCreator.Client.ReleaseCreation;
using ReleaseCreator.Client.ReleaseCreation.GitHub;
using ReleaseCreator.Client.Util;
using ReleaseCreator.Client.VersionCalculation;
using Application = ReleaseCreator.Client.Bootstrap.Application;

namespace ReleaseCreator.Client.Extensions;

/// <summary>
/// Contains extension methods for <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtension
{
    /// <summary>
    /// Gets or sets the factory for creating an <see cref="IReleasesClient"/>.
    /// </summary>
    public static Func<string, IReleasesClient> ReleasesClientFactory { get; set; } = GetReleasesClient;

    /// <summary>
    /// Adds all services for the client.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
    /// <param name="accessToken">The access token.</param>
    public static IServiceCollection AddReleaseCreatorClientServicesSingleton(this IServiceCollection services, string accessToken)
    {
        var client = ReleasesClientFactory(accessToken);

        services.AddSingleton<IEnvironmentService, EnvironmentService>()
            .AddSingleton<IFileService, FileService>()
            .AddSingleton<INextVersionCalculator, NextVersionCalculator>()
            .AddSingleton(_ => client)
            .AddSingleton<IReleaseCreator, GitHubReleaseCreator>()
            .AddSingleton<Application>();

        return services;
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
}
