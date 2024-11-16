using Octokit;
using ReleaseCreator.CommandLine.Types;

namespace ReleaseCreator.CommandLine.ReleaseCreation
{
    internal interface IReleaseCreator
    {
        public Task<Release> CreateReleaseAsync(ReleaseCreatorOptions releaseCreatorOptions);
    }
}