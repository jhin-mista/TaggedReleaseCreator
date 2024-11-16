using ReleaseCreator.VersionIncrementor.Types;

namespace ReleaseCreator.VersionIncrementor.Builder
{
    internal interface ISemanticVersionDirector
    {
        public SemanticVersion IncrementPreReleaseToStable(SemanticVersion currentVersion, SemanticVersionIncrementDto semanticVersionIncrementDto);

        public SemanticVersion IncrementPreReleaseToPreRelease(SemanticVersion currentVersion, SemanticVersionIncrementDto semanticVersionIncrementDto);

        public SemanticVersion IncrementStableToStable(SemanticVersion currentVersion, SemanticVersionIncrementDto semanticVersionIncrementDto);

        public SemanticVersion IncrementStableToPreRelease(SemanticVersion currentVersion, SemanticVersionIncrementDto semanticVersionIncrementDto);
    }
}