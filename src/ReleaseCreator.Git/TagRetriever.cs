namespace ReleaseCreator.Git;

/// <summary>
/// Contains methods for retrieving git tags.
/// </summary>
public class TagRetriever
{
    private readonly IPowerShellExecutor _powerShellExecutor;

    internal TagRetriever(IPowerShellExecutor powerShellExecutor)
    {
        _powerShellExecutor = powerShellExecutor;
    }

    /// <summary>
    /// Gets the latest tag in a git repository under the specified <paramref name="repositoryPath"/>.
    /// </summary>
    /// <returns>The tag name of the latest tag or <see langword="null"/>, if no tags exist.</returns>
    /// <exception cref="AggregateException"/>
    public string? GetLatestTag(string repositoryPath)
    {
        var script = $@"Set-Location {repositoryPath}
git tag --sort=-v:refname | Select-Object -First 1";

        var results = _powerShellExecutor.Execute(script);

        return results.FirstOrDefault()?.ToString();
    }
}