using ReleaseCreator.SemanticVersionUtil.Types;

namespace ReleaseCreator.SemanticVersionUtil
{
    /// <summary>
    /// Represents a component for incrementing a semantic version.
    /// </summary>
    public interface ISemanticVersionIncrementor
    {
        /// <summary>
        /// Increments the <paramref name="currentVersion"/> with the metadata of <paramref name="semanticVersionIncrementDto"/>.
        /// </summary>
        /// <param name="currentVersion">The current version to increment.</param>
        /// <param name="semanticVersionIncrementDto">The semantic version increment dto containing information about how to increment <paramref name="currentVersion"/>.</param>
        /// <returns>A new incremented <see cref="SemanticVersion"/>.</returns>
        public SemanticVersion Increment(SemanticVersion currentVersion, SemanticVersionIncrementDto semanticVersionIncrementDto);
    }
}