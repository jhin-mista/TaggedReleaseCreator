using ReleaseCreator.VersionIncrementor.Builder;
using ReleaseCreator.VersionIncrementor.Types;

namespace ReleaseCreator.VersionIncrementor
{
    internal class SemanticVersionIncrementor : ISemanticVersionIncrementor
    {
        private readonly ISemanticVersionDirector _director;

        internal SemanticVersionIncrementor(ISemanticVersionDirector director)
        {
            _director = director;
        }

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
}