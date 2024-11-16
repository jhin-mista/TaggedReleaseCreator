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

namespace ReleaseCreator.CommandLine;

/// <summary>
/// Entry class for invocations of this program.
/// </summary>
public class Program
{
    /// <summary>
    /// Main entry point of the program.
    /// </summary>
    /// <param name="args">The command line arguments.</param>
    public static async Task Main(string[] args)
    {
        var rootCommand = GetRootCommand();
        var exitCode = await rootCommand.InvokeAsync(args);
        Environment.Exit(exitCode);
    }

    private static RootCommand GetRootCommand()
    {
        var rootCommand = new RootCommand("CLI utility for creating a github release");
        var releaseCreatorOptionsBinder = new ReleaseCreatorOptionsBinder();

        releaseCreatorOptionsBinder.AddOptionsTo(rootCommand);
        rootCommand.SetHandler(StartReleaseCreationAsync, releaseCreatorOptionsBinder);

        return rootCommand;
    }

    private static async Task StartReleaseCreationAsync(ReleaseCreatorOptions releaseCreatorOptions)
    {
        try
        {
            var serviceProvider = GetServiceProvider(releaseCreatorOptions.AccessToken);
            var releaseCreator = serviceProvider.GetRequiredService<IReleaseCreator>();

            var createdRelease = await releaseCreator.CreateReleaseAsync(releaseCreatorOptions);
            Console.WriteLine($"Created release under the following URL: {createdRelease.HtmlUrl}");
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine("Could not create release due to the following exception:");
            Console.Error.WriteLine(exception.Message);
        }
    }

    private static ServiceProvider GetServiceProvider(string accessToken)
    {
        var userAgent = new ProductHeaderValue("ReleaseCreator");
        var credentials = new Credentials(accessToken);
        var credentialStore = new InMemoryCredentialStore(credentials);
        var connection = new Connection(userAgent, credentialStore);
        var apiConnection = new ApiConnection(connection);
        var client = new ReleasesClient(apiConnection);

        var services = new ServiceCollection();

        services.AddVersionIncrementorServicesSingleton()
            .AddGitServicesSingleton()
            .AddSingleton<IEnvironmentService, EnvironmentService>()
            .AddSingleton<INextVersionCalculator, NextVersionCalculator>()
            .AddSingleton<IReleasesClient, ReleasesClient>(_ => client)
            .AddSingleton<IReleaseCreator, GitHubReleaseCreator>();

        return services.BuildServiceProvider();
    }
}