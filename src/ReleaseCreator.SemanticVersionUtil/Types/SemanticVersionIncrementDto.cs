using ReleaseCreator.SemanticVersionUtil.Enums;

namespace ReleaseCreator.SemanticVersionUtil.Types;

/// <summary>
/// Holds metadata for a semantic version increment.
/// </summary>
/// <param name="SemanticVersionCorePart">The version part to increment.</param>
/// <param name="PreReleaseIdentifier">The optional pre-release identifier.</param>
public record SemanticVersionIncrementDto(
    SemanticVersionCorePart SemanticVersionCorePart,
    string? PreReleaseIdentifier)
{
    /// <summary>
    /// The indicator if this increments to a pre-release.
    /// </summary>
    public bool IsPreRelease => PreReleaseIdentifier != null;
}