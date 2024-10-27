namespace ReleaseCreator.Git.Tag;

/// <summary>
/// Represents a service for retrieving git tags.
/// </summary>
public interface ITagRetriever
{
    /// <summary>
    /// Gets the latest tag in a git repository.
    /// </summary>
    /// <returns>The tag name of the latest tag or <see langword="null"/>, if no tags exist.</returns>
    public string? GetLatestTag();
}