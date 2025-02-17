﻿using ReleaseCreator.Git.ShellExecution;

namespace ReleaseCreator.Git.Tag;

/// <inheritdoc cref="ITagRetriever"/>
internal class TagRetriever(IPowerShellExecutor powerShellExecutor) : ITagRetriever
{
    private readonly IPowerShellExecutor _powerShellExecutor = powerShellExecutor;

    /// <inheritdoc/>
    /// <exception cref="AggregateException"/>
    public string? GetLatestTag()
    {
        var script = "git tag --list \"*.*.*\" --sort=-v:refname --merged | Select-Object -First 1";

        var results = _powerShellExecutor.Execute(script);

        return results.FirstOrDefault()?.ToString();
    }
}