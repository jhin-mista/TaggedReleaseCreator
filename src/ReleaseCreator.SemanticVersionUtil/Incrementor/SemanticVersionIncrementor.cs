using ReleaseCreator.SemanticVersionUtil.Types;

namespace ReleaseCreator.SemanticVersionUtil.Incrementor;

internal class SemanticVersionIncrementor(ISemanticVersionIncrementDirector director) : ISemanticVersionIncrementor
{
    private readonly ISemanticVersionIncrementDirector _director = director;

    /// <inheritdoc/>
    public SemanticVersion Increment(SemanticVersion currentVersion, SemanticVersionIncrementDto semanticVersionIncrementDto)
    {
        if (currentVersion.IsStableVersion && semanticVersionIncrementDto.IsPreRelease)
        {
            return _director.IncrementStableToPreRelease(currentVersion, semanticVersionIncrementDto);
        }

        else if (currentVersion.IsPreRelease && semanticVersionIncrementDto.IsPreRelease)
        {
            return _director.IncrementPreReleaseToPreRelease(currentVersion, semanticVersionIncrementDto);
        }

        else if (currentVersion.IsStableVersion && semanticVersionIncrementDto.IsStableVersion)
        {
            return _director.IncrementStableToStable(currentVersion, semanticVersionIncrementDto);
        }

        return _director.IncrementPreReleaseToStable(currentVersion, semanticVersionIncrementDto);
    }
}