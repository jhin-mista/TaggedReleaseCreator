using Microsoft.Extensions.DependencyInjection;
using Octokit;
using Octokit.Internal;
using ReleaseCreator.CommandLine.ReleaseCreation;
using ReleaseCreator.CommandLine.ReleaseCreation.GitHub;
using ReleaseCreator.CommandLine.Types;
using ReleaseCreator.CommandLine.Util;
using ReleaseCreator.CommandLine.VersionCalculation;
using ReleaseCreator.Git.Extensions;
using ReleaseCreator.SemanticVersionUtil.Extensions;
using System.CommandLine;
using System.Diagnostics.CodeAnalysis;

namespace ReleaseCreator.CommandLine;

/// <summary>
/// Entry class for invocations of this program.
/// </summary>
public class Program
{
    internal static Func<string, IReleasesClient> ReleasesClientFactory { get; set; } = GetReleasesClient;

    /// <summary>
    /// Main entry point of the program.
    /// </summary>
    /// <param name="args">The command line arguments.</param>
    public static async Task<int> Main(string[] args)
    {
        if (TryGetReleaseCreatorOptions(args, out var releaseCreatorOptions))
        {
            var serviceProvider = SetupServices(releaseCreatorOptions.AccessToken);
            var releaseCreator = serviceProvider.GetRequiredService<IReleaseCreator>();

            var createdRelease = await releaseCreator.CreateReleaseAsync(releaseCreatorOptions);
            Console.WriteLine($"Created release under the following URL: {createdRelease.HtmlUrl}");
        }
        else
        {
            return -1;
        }

        return 0;
    }

    private static bool TryGetReleaseCreatorOptions(string[] args, [NotNullWhen(true)] out ReleaseCreatorOptions? result)
    {
        var rootCommand = new RootCommand("CLI utility for creating a github release");
        var releaseCreatorOptionsBinder = new ReleaseCreatorOptionsBinder();

        releaseCreatorOptionsBinder.AddOptionsTo(rootCommand);
        ReleaseCreatorOptions? releaseCreatorOptions = null;
        rootCommand.SetHandler(x => releaseCreatorOptions = x, releaseCreatorOptionsBinder);

        var exitCode = rootCommand.Invoke(args);

        result = releaseCreatorOptions;
        if (exitCode != 0 || result == null)
        {
            return false;
        }

        return true;
    }

    private static ServiceProvider SetupServices(string accessToken)
    {
        var client = ReleasesClientFactory(accessToken);
        var services = new ServiceCollection();

        services.AddVersionIncrementorServicesSingleton()
            .AddGitServicesSingleton()
            .AddSingleton<IEnvironmentService, EnvironmentService>()
            .AddSingleton<INextVersionCalculator, NextVersionCalculator>()
            .AddSingleton(_ => client)
            .AddSingleton<IReleaseCreator, GitHubReleaseCreator>();

        return services.BuildServiceProvider();
    }

    private static IReleasesClient GetReleasesClient(string accessToken)
    {
        var userAgent = new ProductHeaderValue("ReleaseCreator");
        var credentials = new Credentials(accessToken);
        var credentialStore = new InMemoryCredentialStore(credentials);
        var connection = new Connection(userAgent, credentialStore);
        var apiConnection = new ApiConnection(connection);
        return new ReleasesClient(apiConnection);
    }
}