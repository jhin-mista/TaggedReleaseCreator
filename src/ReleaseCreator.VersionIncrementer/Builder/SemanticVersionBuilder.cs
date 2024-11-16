using ReleaseCreator.VersionIncrementor.Types;

namespace ReleaseCreator.VersionIncrementor.Builder
{
    internal class SemanticVersionBuilder : ISemanticVersionBuilder
    {
        private uint _major;
        private uint _minor;
        private uint _patch;

        IList<string>? _buildMetadata;

        IList<string>? _preReleaseIdentifier;
        uint? _preReleaseNumber;

        public SemanticVersion GetSemanticVersion()
        {
            return new(_major, _minor, _patch, _preReleaseIdentifier, _preReleaseNumber, _buildMetadata);
        }

        public void SetBuildMetadata(IList<string>? buildMetadata)
        {
            _buildMetadata = buildMetadata;
        }

        public void SetMajorVersion(uint majorVersion)
        {
            _major = majorVersion;
        }

        public void SetMinorVersion(uint minorVersion)
        {
            _minor = minorVersion;
        }

        public void SetPatchVersion(uint patchVersion)
        {
            _patch = patchVersion;
        }

        public void SetPreReleaseIdentifier(IList<string>? preReleaseIdentifier)
        {
            _preReleaseIdentifier = preReleaseIdentifier;
        }

        public void SetPreReleaseNumber(uint? preReleaseNumber)
        {
            _preReleaseNumber = preReleaseNumber;
        }
    }
}
