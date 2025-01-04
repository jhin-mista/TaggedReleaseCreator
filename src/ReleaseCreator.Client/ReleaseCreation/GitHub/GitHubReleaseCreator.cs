﻿using Microsoft.Extensions.Logging;
using Octokit;
using ReleaseCreator.Client.Types;
using ReleaseCreator.Client.Util;
using ReleaseCreator.Client.VersionCalculation;

namespace ReleaseCreator.Client.ReleaseCreation.GitHub;

internal class GitHubReleaseCreator(
    IReleasesClient releasesClient,
    INextVersionCalculator nextVersionCalculator,
    IEnvironmentService environmentService,
    IFileService fileService,
    ILogger<GitHubReleaseCreator> logger) : IReleaseCreator
{
    private readonly IReleasesClient _releasesClient = releasesClient;
    private readonly INextVersionCalculator _nextVersionCalculator = nextVersionCalculator;
    private readonly IEnvironmentService _environmentService = environmentService;
    private readonly IFileService _fileService = fileService;
    private readonly ILogger<GitHubReleaseCreator> _logger = logger;

    public async Task<Release> CreateReleaseAsync(ReleaseCreatorOptions releaseCreatorOptions)
    {
        var nextVersion = _nextVersionCalculator.CalculateNextVersion(releaseCreatorOptions);

        var nextRelease = new NewRelease(nextVersion.ToStringWithPrefix())
        {
            Prerelease = releaseCreatorOptions.IsPreRelease,
            TargetCommitish = releaseCreatorOptions.CommitSha,
        };

        var repositoryId = GetRepositoryId();

        _logger.LogDebug("New tag name: {tagName}", nextRelease.TagName);

        SetNextVersionOutput(nextRelease.TagName);

        return await _releasesClient.Create(repositoryId, nextRelease);
    }

    private void SetNextVersionOutput(string nextVersion)
    {
        var outputFilePathVariableName = KnownConstants.GitHub.EnvironmentVariables.OutputFilePath;

        var outputFilePath = _environmentService.GetRequiredEnvironmentVariable(outputFilePathVariableName);

        _fileService.AppendLine(outputFilePath, $"{KnownConstants.GitHub.Action.NextVersionOutputVariableName}={nextVersion}");
    }

    private long GetRepositoryId()
    {
        var repositoryIdVariableName = KnownConstants.GitHub.EnvironmentVariables.RepositoryId;

        var repositoryId = _environmentService.GetRequiredEnvironmentVariable(repositoryIdVariableName);

        return long.TryParse(repositoryId, out var result)
            ? result
            : throw new Exception($"Expected environment variable '{repositoryIdVariableName}' to be of type '{typeof(long)}' but '{repositoryId}' cannot be parsed.");
    }
}