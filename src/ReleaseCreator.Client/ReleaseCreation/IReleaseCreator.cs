using ReleaseCreator.Client.Types;

namespace ReleaseCreator.Client.ReleaseCreation;

/// <summary>
/// Represents a component for creating a release.
/// </summary>
public interface IReleaseCreator
{
    /// <summary>
    /// Creates a new release asynchronously based on the provided options.
    /// </summary>
    /// <param name="releaseCreatorOptions">Specifies the details and configuration for the new release.</param>
    public Task CreateReleaseAsync(ReleaseCreatorOptions releaseCreatorOptions);
}