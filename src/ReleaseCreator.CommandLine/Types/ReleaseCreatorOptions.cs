namespace ReleaseCreator.CommandLine.Types;

internal record ReleaseCreatorOptions(
    string BranchName,
    SemanticVersionPart VersionIncreasePart,
    string? PreReleaseIdentifier,
    string AccessToken)
{
}