using ReleaseCreator.CommandLine.Types;
using ReleaseCreator.SemanticVersionUtil.Types;

namespace ReleaseCreator.CommandLine;

/// <summary>
/// Represents a component for calculating the next semantic version in a git repository.
/// </summary>
internal interface INextVersionCalculator
{
    /// <summary>
    /// Calculates the next semantic version based on the given <paramref name="releaseCreatorOptions"/>.
    /// </summary>
    /// <param name="releaseCreatorOptions">Contains information on how the next version should be calculated.</param>
    public SemanticVersion CalculateNextVersion(ReleaseCreatorOptions releaseCreatorOptions);
}