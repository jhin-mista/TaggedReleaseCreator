using ReleaseCreator.CommandLine.Enums;
using System.Text;

namespace ReleaseCreator.CommandLine.Types;

/// <summary>
/// Holds options for creating a release, including versioning and access details.
/// </summary>
/// <param name="Commitish">Specifies the unique identifier for the commit associated with the release.</param>
/// <param name="SemanticReleaseType">Indicates the part of the version number that should be incremented.</param>
/// <param name="PreReleaseIdentifier">Allows for the inclusion of an identifier for pre-release versions.</param>
/// <param name="AccessToken">Provides the authentication token required for accessing the release creation functionality.</param>
internal record ReleaseCreatorCommandLineOptions(
    string Commitish,
    SemanticReleaseType SemanticReleaseType,
    string? PreReleaseIdentifier,
    string AccessToken)
{
    /// <summary>
    /// Indicates if this is a pre-release.
    /// </summary>
    internal bool IsPreRelease => PreReleaseIdentifier != null;

    /// <summary>
    /// Appends member information to the provided <paramref name="builder"/>.
    /// </summary>
    /// <param name="builder">The string builder used to accumulate formatted member data.</param>
    protected virtual bool PrintMembers(StringBuilder builder)
    {
        builder.Append($"{nameof(Commitish)} = {Commitish}, {nameof(SemanticReleaseType)} = {SemanticReleaseType}, {nameof(PreReleaseIdentifier)} = {PreReleaseIdentifier}, {nameof(IsPreRelease)} = {IsPreRelease}");

        return true;
    }
}