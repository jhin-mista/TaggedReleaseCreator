using ReleaseCreator.SemanticVersionUtil.Types;

namespace ReleaseCreator.SemanticVersionUtil.Parser
{
    /// <summary>
    /// Represents a component for parsing semantic versions.
    /// </summary>
    public interface ISemanticVersionParser
    {
        /// <summary>
        /// Parses a <see cref="string"/> into a <see cref="SemanticVersion"/>.
        /// </summary>
        /// <param name="semanticVersionString">The string containing a semantic version.</param>
        public SemanticVersion Parse(string semanticVersionString);
    }
}