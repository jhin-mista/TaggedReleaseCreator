using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
        if (!TryGetReleaseCreatorOptions(args, out var releaseCreatorOptions))
        {
            return -1;
        }

        var serviceProvider = SetupServices(releaseCreatorOptions.AccessToken);
        var releaseCreator = serviceProvider.GetRequiredService<IReleaseCreator>();
        var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

        try
        {
            var passedArguments = string.Join(", ", args);
            logger.LogDebug("Input arguments: {arguments}", passedArguments);

            var createdRelease = await releaseCreator.CreateReleaseAsync(releaseCreatorOptions);
            logger.LogInformation("Created release under the following URL: {releaseUrl}", createdRelease.HtmlUrl);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception while creating release");

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

        return exitCode == 0 && result != null;
    }

    private static ServiceProvider SetupServices(string accessToken)
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

    private static void AddConsoleLoggerWithLogLevelSetByEnvironmentVariable(ILoggingBuilder builder)
    {
        var config = new ConfigurationBuilder().AddEnvironmentVariables().Build();

        builder.AddConsole()
            .AddConfiguration(config.GetSection("Logging"));
    }
}