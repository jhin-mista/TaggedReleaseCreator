using Octokit;
using ReleaseCreator.Client.Types;

namespace ReleaseCreator.Client.ReleaseCreation;

internal interface IReleaseCreator
{
    public Task<Release> CreateReleaseAsync(ReleaseCreatorOptions releaseCreatorOptions);
}