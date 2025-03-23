using Microsoft.Extensions.DependencyInjection;
using ReleaseCreator.CommandLine.Types;
using System.CommandLine;
using System.Diagnostics.CodeAnalysis;

namespace ReleaseCreator.CommandLine.Bootstrap;

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
        if (!TryGetReleaseCreatorCommandLineOptions(args, out var releaseCreatorCommandLineOptions))
        {
            Environment.ExitCode = -1;
            return;
        }

        var serviceProvider = ContainerBootstrapper.BuildUp(releaseCreatorCommandLineOptions.AccessToken);
        var application = serviceProvider.GetRequiredService<Application>();

        await application.RunAsync(releaseCreatorCommandLineOptions);
    }

    private static bool TryGetReleaseCreatorCommandLineOptions(string[] args, [NotNullWhen(true)] out ReleaseCreatorCommandLineOptions? result)
    {
        var rootCommand = new RootCommand("CLI utility for creating a GitHub release");
        var releaseCreatorOptionsBinder = new ReleaseCreatorOptionsBinder();

        releaseCreatorOptionsBinder.AddOptionsTo(rootCommand);
        ReleaseCreatorCommandLineOptions? releaseCreatorOptions = null;
        rootCommand.SetHandler(x => releaseCreatorOptions = x, releaseCreatorOptionsBinder);

        var exitCode = rootCommand.Invoke(args);
        result = releaseCreatorOptions;

        return exitCode == 0 && result != null;
    }
}