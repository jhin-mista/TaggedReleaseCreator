using ReleaseCreator.CommandLine.Enums;

namespace ReleaseCreator.CommandLine.Types;

internal record ReleaseCreatorOptions(
    string CommitSha,
    ReleaseType VersionIncreasePart,
    string? PreReleaseIdentifier,
    bool IsPreRelease,
    string AccessToken)
{
}