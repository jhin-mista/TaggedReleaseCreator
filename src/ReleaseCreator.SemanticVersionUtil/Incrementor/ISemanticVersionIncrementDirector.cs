using ReleaseCreator.SemanticVersionUtil.Types;

namespace ReleaseCreator.SemanticVersionUtil.Incrementor;

internal interface ISemanticVersionIncrementDirector
{
    public SemanticVersion IncrementPreReleaseToStable(SemanticVersion currentVersion, SemanticVersionIncrementDto semanticVersionIncrementDto);

    public SemanticVersion IncrementPreReleaseToPreRelease(SemanticVersion currentVersion, SemanticVersionIncrementDto semanticVersionIncrementDto);

    public SemanticVersion IncrementStableToStable(SemanticVersion currentVersion, SemanticVersionIncrementDto semanticVersionIncrementDto);

    public SemanticVersion IncrementStableToPreRelease(SemanticVersion currentVersion, SemanticVersionIncrementDto semanticVersionIncrementDto);
}