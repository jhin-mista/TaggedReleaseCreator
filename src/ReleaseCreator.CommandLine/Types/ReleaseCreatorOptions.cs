using ReleaseCreator.CommandLine.Enums;

namespace ReleaseCreator.CommandLine.Types;

internal record ReleaseCreatorOptions(
    string BranchName,
    ReleaseType VersionIncreasePart,
    string? PreReleaseIdentifier,
    string AccessToken)
{
}