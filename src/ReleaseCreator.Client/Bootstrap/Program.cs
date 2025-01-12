using Microsoft.Extensions.DependencyInjection;
using ReleaseCreator.Client.Types;
using System.CommandLine;
using System.Diagnostics.CodeAnalysis;

namespace ReleaseCreator.Client.Bootstrap;

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
        if (!TryGetReleaseCreatorOptions(args, out var releaseCreatorOptions))
        {
            Environment.ExitCode = -1;
            return;
        }

        var serviceProvider = ContainerBootstrapper.BuildUp(releaseCreatorOptions.AccessToken);
        var application = serviceProvider.GetRequiredService<Application>();

        await application.RunAsync(releaseCreatorOptions);
    }

    private static bool TryGetReleaseCreatorOptions(string[] args, [NotNullWhen(true)] out ReleaseCreatorOptions? result)
    {
        var rootCommand = new RootCommand("CLI utility for creating a GitHub release");
        var releaseCreatorOptionsBinder = new ReleaseCreatorOptionsBinder();

        releaseCreatorOptionsBinder.AddOptionsTo(rootCommand);
        ReleaseCreatorOptions? releaseCreatorOptions = null;
        rootCommand.SetHandler(x => releaseCreatorOptions = x, releaseCreatorOptionsBinder);

        var exitCode = rootCommand.Invoke(args);
        result = releaseCreatorOptions;

        return exitCode == 0 && result != null;
    }
}