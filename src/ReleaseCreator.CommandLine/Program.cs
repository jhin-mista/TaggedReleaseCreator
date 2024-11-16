using Microsoft.Extensions.Hosting;
using ReleaseCreator.CommandLine.Types;
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
        var builder = Host.CreateApplicationBuilder();
        SetupServices(builder);
        var host = builder.Build();

        var rootCommand = new RootCommand("CLI utility for creating a github release");
        var releaseCreatorOptionsBinder = new ReleaseCreatorOptionsBinder();

        releaseCreatorOptionsBinder.AddOptionsTo(rootCommand);
        rootCommand.SetHandler(x => CreateRelease(x, host), releaseCreatorOptionsBinder);

        return rootCommand;
    }

    private static void CreateRelease(ReleaseCreatorOptions releaseCreatorOptions, IHost host)
    {
        Console.WriteLine(releaseCreatorOptions.ToString());
    }

    private static void SetupServices(HostApplicationBuilder hostBuilder)
    {
        hostBuilder.Services
            .AddVersionIncrementorServicesSingleton()
            .AddGitServicesSingleton();
    }
}