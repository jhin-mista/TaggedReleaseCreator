using Microsoft.Extensions.Logging;
using ReleaseCreator.Client.ReleaseCreation;
using ReleaseCreator.Client.Types;
using ReleaseCreator.CommandLine.Types;
using ReleaseCreatorSemanticReleaseType = ReleaseCreator.Client.Enums.SemanticReleaseType;
using SemanticReleaseType = ReleaseCreator.CommandLine.Enums.SemanticReleaseType;

namespace ReleaseCreator.CommandLine.Bootstrap;

/// <summary>
/// Entry point of the application logic.
/// </summary>
/// <remarks>
/// Creates an instance of <see cref="Application"/>.
/// </remarks>
/// <param name="logger">The logger.</param>
/// <param name="releaseCreator">The release creator.</param>
internal class Application(ILogger<Application> logger, IReleaseCreator releaseCreator)
{
    private readonly ILogger<Application> _logger = logger;
    private readonly IReleaseCreator _releaseCreator = releaseCreator;

    /// <summary>
    /// Runs the application.
    /// </summary>
    /// <param name="releaseCreatorCommandLineOptions">The release creator command line options.</param>
    internal async Task RunAsync(ReleaseCreatorCommandLineOptions releaseCreatorCommandLineOptions)
    {
        try
        {
            _logger.LogDebug("Input arguments: {arguments}", releaseCreatorCommandLineOptions);

            var releaseCreatorOptions = CreateReleaseCreatorOptions(releaseCreatorCommandLineOptions);
            await _releaseCreator.CreateReleaseAsync(releaseCreatorOptions);

            Environment.ExitCode = 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception while creating release");

            Environment.ExitCode = -1;
        }
    }

    private static ReleaseCreatorOptions CreateReleaseCreatorOptions(ReleaseCreatorCommandLineOptions releaseCreatorCommandLineOptions)
    {
        var semanticReleaseType = CreateSemanticReleaseType(releaseCreatorCommandLineOptions.SemanticReleaseType);

        return new(releaseCreatorCommandLineOptions.Commitish, semanticReleaseType, releaseCreatorCommandLineOptions.PreReleaseIdentifier);
    }

    private static ReleaseCreatorSemanticReleaseType CreateSemanticReleaseType(SemanticReleaseType releaseType)
    {
        return releaseType switch
        {
            SemanticReleaseType.Major => ReleaseCreatorSemanticReleaseType.Major,
            SemanticReleaseType.Minor => ReleaseCreatorSemanticReleaseType.Minor,
            SemanticReleaseType.Patch => ReleaseCreatorSemanticReleaseType.Patch,
            _ => throw new NotSupportedException($"{nameof(SemanticReleaseType)} {releaseType} cannot be mapped to a {nameof(ReleaseCreatorSemanticReleaseType)}")
        };
    }
}