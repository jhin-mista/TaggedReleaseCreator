using ReleaseCreator.VersionIncrementor.Enums;
using ReleaseCreator.VersionIncrementor.Types;

namespace ReleaseCreator.VersionIncrementor.Builder
{
    internal class SemanticVersionDirector : ISemanticVersionDirector
    {
        private readonly ISemanticVersionBuilder _builder;

        internal SemanticVersionDirector(ISemanticVersionBuilder builder)
        {
            _builder = builder;
        }

        public SemanticVersion IncrementPreReleaseToPreRelease(SemanticVersion currentVersion, SemanticVersionIncrementDto semanticVersionIncrementDto)
        {
            _builder.SetBuildMetadata(currentVersion.BuildMetadata);
            SetCoreVersion(currentVersion.Major, currentVersion.Minor, currentVersion.Patch);

            if (IsNewPreReleaseIdentifier(currentVersion, semanticVersionIncrementDto))
            {
                SetIncrementedPreReleaseNumber(currentVersion);
            }
            else
            {
                _builder.SetPreReleaseNumber(1);
            }

            SetPreReleaseIdentifier(currentVersion, semanticVersionIncrementDto.PreReleaseIdentifier);

            return _builder.GetSemanticVersion();
        }

        public SemanticVersion IncrementPreReleaseToStable(SemanticVersion currentVersion, SemanticVersionIncrementDto semanticVersionIncrementDto)
        {
            _builder.SetBuildMetadata(currentVersion.BuildMetadata);
            SetCoreVersion(currentVersion.Major, currentVersion.Minor, currentVersion.Patch);

            return _builder.GetSemanticVersion();
        }

        public SemanticVersion IncrementStableToPreRelease(SemanticVersion currentVersion, SemanticVersionIncrementDto semanticVersionIncrementDto)
        {
            _builder.SetBuildMetadata(currentVersion.BuildMetadata);
            SetIncrementedCoreVersion(currentVersion, semanticVersionIncrementDto.SemanticVersionCorePart);
            SetIncrementedPreReleaseNumber(currentVersion);
            SetPreReleaseIdentifier(currentVersion, semanticVersionIncrementDto.PreReleaseIdentifier);

            return _builder.GetSemanticVersion();
        }

        public SemanticVersion IncrementStableToStable(SemanticVersion currentVersion, SemanticVersionIncrementDto semanticVersionIncrementDto)
        {
            _builder.SetBuildMetadata(currentVersion.BuildMetadata);
            SetIncrementedCoreVersion(currentVersion, semanticVersionIncrementDto.SemanticVersionCorePart);

            return _builder.GetSemanticVersion();
        }

        private static bool IsNewPreReleaseIdentifier(SemanticVersion currentVersion, SemanticVersionIncrementDto semanticVersionIncrementDto)
        {
            return currentVersion.PreReleaseIdentifier?.Last() == semanticVersionIncrementDto.PreReleaseIdentifier || semanticVersionIncrementDto.PreReleaseIdentifier == null;
        }

        private void SetIncrementedCoreVersion(SemanticVersion currentVersion, SemanticVersionCorePart semanticVersionCorePart)
        {
            switch (semanticVersionCorePart)
            {
                case SemanticVersionCorePart.Major:
                    SetCoreVersion(currentVersion.Major + 1, 0, 0);
                    break;
                case SemanticVersionCorePart.Minor:
                    SetCoreVersion(currentVersion.Major, currentVersion.Minor + 1, 0);
                    break;
                case SemanticVersionCorePart.Patch:
                    SetCoreVersion(currentVersion.Major, currentVersion.Minor, currentVersion.Patch + 1);
                    break;
                default:
                    throw new NotSupportedException($"Cannot increment {nameof(SemanticVersionCorePart)} {semanticVersionCorePart}.");
            }
        }

        private void SetCoreVersion(uint major, uint minor, uint patch)
        {
            _builder.SetMajorVersion(major);
            _builder.SetMinorVersion(minor);
            _builder.SetPatchVersion(patch);
        }

        private void SetIncrementedPreReleaseNumber(SemanticVersion currentVersion)
        {
            var newPreReleaseNumber = (currentVersion.PreReleaseNumber ?? 0) + 1;

            _builder.SetPreReleaseNumber(newPreReleaseNumber);
        }

        private void SetPreReleaseIdentifier(SemanticVersion currentVersion, string? newPreReleaseIdentifier)
        {
            if (newPreReleaseIdentifier == null)
            {
                _builder.SetPreReleaseIdentifier(currentVersion.PreReleaseIdentifier);
            }
            else
            {
                _builder.SetPreReleaseIdentifier([newPreReleaseIdentifier]);
            }
        }
    }
}