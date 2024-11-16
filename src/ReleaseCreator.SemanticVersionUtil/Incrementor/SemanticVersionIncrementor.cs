using ReleaseCreator.SemanticVersionUtil.Types;

namespace ReleaseCreator.SemanticVersionUtil.Incrementor;

internal class SemanticVersionIncrementor(ISemanticVersionIncrementDirector director) : ISemanticVersionIncrementor
{
    private readonly ISemanticVersionIncrementDirector _director = director;

    /// <inheritdoc/>
    public SemanticVersion Increment(SemanticVersion currentVersion, SemanticVersionIncrementDto semanticVersionIncrementDto)
    {
        if (IsStableVersion(currentVersion) && semanticVersionIncrementDto.IsPreRelease)
        {
            return _director.IncrementStableToPreRelease(currentVersion, semanticVersionIncrementDto);
        }

        if (!IsStableVersion(currentVersion) && semanticVersionIncrementDto.IsPreRelease)
        {
            return _director.IncrementPreReleaseToPreRelease(currentVersion, semanticVersionIncrementDto);
        }

        if (IsStableVersion(currentVersion) && !semanticVersionIncrementDto.IsPreRelease)
        {
            return _director.IncrementStableToStable(currentVersion, semanticVersionIncrementDto);
        }

        return _director.IncrementPreReleaseToStable(currentVersion, semanticVersionIncrementDto);
    }

    private static bool IsStableVersion(SemanticVersion version)
    {
        return version.PreReleaseIdentifier == null || version.PreReleaseIdentifier.Count == 0;
    }
}