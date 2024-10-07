using Microsoft.Extensions.Logging;
using Octokit;
using ReleaseCreator.CommandLine.Types;
using ReleaseCreator.CommandLine.Util;
using ReleaseCreator.CommandLine.VersionCalculation;

namespace ReleaseCreator.CommandLine.ReleaseCreation.GitHub;

internal class GitHubReleaseCreator(
    IReleasesClient releasesClient,
    INextVersionCalculator nextVersionCalculator,
    IEnvironmentService environmentService,
    ILogger<GitHubReleaseCreator> logger) : IReleaseCreator
{
    private readonly IReleasesClient _releasesClient = releasesClient;
    private readonly INextVersionCalculator _nextVersionCalculator = nextVersionCalculator;
    private readonly IEnvironmentService _environmentService = environmentService;
    private readonly ILogger<GitHubReleaseCreator> _logger = logger;

    public async Task<Release> CreateReleaseAsync(ReleaseCreatorOptions releaseCreatorOptions)
    {
        var nextVersion = _nextVersionCalculator.CalculateNextVersion(releaseCreatorOptions);

        var nextRelease = new NewRelease("v" + nextVersion.ToString())
        {
            Prerelease = releaseCreatorOptions.IsPreRelease,
            TargetCommitish = releaseCreatorOptions.BranchName,
        };

        var repositoryId = GetRepositoryId();

        _logger.LogDebug("New tag name: {tagName}", nextRelease.TagName);

        return await _releasesClient.Create(repositoryId, nextRelease);
    }

    private long GetRepositoryId()
    {
        const string RepositoryIdVariableName = "GITHUB_REPOSITORY_ID";

        var repositoryId = _environmentService.GetEnvironmentVariable(RepositoryIdVariableName)
            ?? throw new Exception($"Environment variable '{RepositoryIdVariableName}' is not set but required.");

        return long.TryParse(repositoryId, out var result)
            ? result
            : throw new Exception($"Expected environment variable '{RepositoryIdVariableName}' to be of type '{typeof(long)}' but '{repositoryId}' cannot be parsed.");
    }
}