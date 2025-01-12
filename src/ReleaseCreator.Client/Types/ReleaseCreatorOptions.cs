using ReleaseCreator.Client.Enums;
using System.Text;

namespace ReleaseCreator.Client.Types;

internal record ReleaseCreatorOptions(
    string CommitSha,
    ReleaseType VersionIncreasePart,
    string? PreReleaseIdentifier,
    string AccessToken)
{
    internal bool IsPreRelease => PreReleaseIdentifier != null;

    protected virtual bool PrintMembers(StringBuilder builder)
    {
        builder.Append($"{nameof(CommitSha)} = {CommitSha}, {nameof(VersionIncreasePart)} = {VersionIncreasePart}, {nameof(PreReleaseIdentifier)} = {PreReleaseIdentifier}, {nameof(IsPreRelease)} = {IsPreRelease}");

        return true;
    }
}