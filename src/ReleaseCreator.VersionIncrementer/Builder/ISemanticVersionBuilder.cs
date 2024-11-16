using ReleaseCreator.VersionIncrementor.Types;

namespace ReleaseCreator.VersionIncrementor.Builder
{
    internal interface ISemanticVersionBuilder
    {
        internal void SetMajorVersion(uint majorVersion);

        internal void SetMinorVersion(uint minorVersion);

        internal void SetPatchVersion(uint patchVersion);

        internal void SetPreReleaseNumber(uint? preReleaseNumber);

        internal void SetPreReleaseIdentifier(IList<string>? preReleaseIdentifier);

        internal void SetBuildMetadata(IList<string>? buildMetadata);

        internal SemanticVersion GetSemanticVersion();
    }
}