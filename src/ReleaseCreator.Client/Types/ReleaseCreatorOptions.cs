using ReleaseCreator.Client.Enums;

namespace ReleaseCreator.Client.Types;

internal record ReleaseCreatorOptions(
    string CommitSha,
    ReleaseType VersionIncreasePart,
    string? PreReleaseIdentifier,
    bool IsPreRelease,
    string AccessToken)
{
}