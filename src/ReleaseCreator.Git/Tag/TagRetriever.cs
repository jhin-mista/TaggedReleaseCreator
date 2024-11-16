using ReleaseCreator.Git.ShellExecution;

namespace ReleaseCreator.Git.Tag;

/// <inheritdoc cref="ITagRetriever"/>
public class TagRetriever : ITagRetriever
{
    private readonly IPowerShellExecutor _powerShellExecutor;

    internal TagRetriever(IPowerShellExecutor powerShellExecutor)
    {
        _powerShellExecutor = powerShellExecutor;
    }

    /// <inheritdoc/>
    /// <exception cref="AggregateException"/>
    public string? GetLatestTag()
    {
        var script = "git tag --sort=-v:refname | Select-Object -First 1";

        var results = _powerShellExecutor.Execute(script);

        return results.FirstOrDefault()?.ToString();
    }
}