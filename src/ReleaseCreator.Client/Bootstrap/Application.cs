using Microsoft.Extensions.Logging;
using ReleaseCreator.Client.ReleaseCreation;
using ReleaseCreator.Client.Types;

namespace ReleaseCreator.Client.Bootstrap;

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
    /// <param name="releaseCreatorOptions">The release creator options.</param>
    internal async Task RunAsync(ReleaseCreatorOptions releaseCreatorOptions)
    {
        try
        {
            _logger.LogDebug("Input arguments: {arguments}", releaseCreatorOptions);

            var createdRelease = await _releaseCreator.CreateReleaseAsync(releaseCreatorOptions);

            _logger.LogInformation("Created release under the following URL: {releaseUrl}", createdRelease.HtmlUrl);

            Environment.ExitCode = 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception while creating release");

            Environment.ExitCode = -1;
        }
    }
}