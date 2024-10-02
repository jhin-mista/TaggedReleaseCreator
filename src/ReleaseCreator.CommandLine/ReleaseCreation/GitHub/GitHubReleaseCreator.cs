using Octokit;
using ReleaseCreator.CommandLine.Types;
using ReleaseCreator.CommandLine.Util;
using ReleaseCreator.CommandLine.VersionCalculation;

namespace ReleaseCreator.CommandLine.ReleaseCreation.GitHub;

internal class GitHubReleaseCreator : IReleaseCreator
{
    private readonly IReleasesClient _releasesClient;
    private readonly INextVersionCalculator _nextVersionCalculator;
    private readonly IEnvironmentService _environmentService;

    public GitHubReleaseCreator(IReleasesClient releasesClient, INextVersionCalculator nextVersionCalculator, IEnvironmentService environmentService)
    {
        _releasesClient = releasesClient;
        _nextVersionCalculator = nextVersionCalculator;
        _environmentService = environmentService;
    }

    public async Task<Release> CreateReleaseAsync(ReleaseCreatorOptions releaseCreatorOptions)
    {
        var nextVersion = _nextVersionCalculator.CalculateNextVersion(releaseCreatorOptions);

        var nextRelease = new NewRelease("v" + nextVersion.ToString())
        {
            Prerelease = releaseCreatorOptions.IsPreRelease,
            TargetCommitish = releaseCreatorOptions.BranchName,
        };

        var repositoryId = GetRepositoryId();

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