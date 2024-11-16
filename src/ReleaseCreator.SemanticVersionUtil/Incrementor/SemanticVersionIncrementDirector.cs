using ReleaseCreator.SemanticVersionUtil.Builder;
using ReleaseCreator.SemanticVersionUtil.Enums;
using ReleaseCreator.SemanticVersionUtil.Types;

namespace ReleaseCreator.SemanticVersionUtil.Incrementor;

internal class SemanticVersionIncrementDirector(ISemanticVersionBuilder builder) : ISemanticVersionIncrementDirector
{
    private readonly ISemanticVersionBuilder _builder = builder;

    public SemanticVersion IncrementPreReleaseToPreRelease(SemanticVersion currentVersion, SemanticVersionIncrementDto semanticVersionIncrementDto)
    {
        _builder.SetBuildMetadata(currentVersion.BuildMetadata);
        _builder.SetPrefix(currentVersion.Prefix);
        SetCoreVersion(currentVersion.Major, currentVersion.Minor, currentVersion.Patch);

        if (IsNewPreReleaseIdentifier(currentVersion, semanticVersionIncrementDto))
        {
            _builder.SetPreReleaseIdentifier(semanticVersionIncrementDto.PreReleaseIdentifier);
            _builder.SetPreReleaseNumber(1);
        }
        else
        {
            _builder.SetPreReleaseIdentifier(currentVersion.PreReleaseIdentifier);
            SetIncrementedPreReleaseNumber(currentVersion);
        }

        return _builder.BuildSemanticVersion();
    }

    public SemanticVersion IncrementPreReleaseToStable(SemanticVersion currentVersion, SemanticVersionIncrementDto semanticVersionIncrementDto)
    {
        _builder.SetBuildMetadata(currentVersion.BuildMetadata);
        _builder.SetPrefix(currentVersion.Prefix);
        SetCoreVersion(currentVersion.Major, currentVersion.Minor, currentVersion.Patch);

        return _builder.BuildSemanticVersion();
    }

    public SemanticVersion IncrementStableToPreRelease(SemanticVersion currentVersion, SemanticVersionIncrementDto semanticVersionIncrementDto)
    {
        _builder.SetBuildMetadata(currentVersion.BuildMetadata);
        _builder.SetPrefix(currentVersion.Prefix);
        SetIncrementedCoreVersion(currentVersion, semanticVersionIncrementDto.SemanticVersionCorePart);
        SetIncrementedPreReleaseNumber(currentVersion);
        _builder.SetPreReleaseIdentifier(semanticVersionIncrementDto.PreReleaseIdentifier);

        return _builder.BuildSemanticVersion();
    }

    public SemanticVersion IncrementStableToStable(SemanticVersion currentVersion, SemanticVersionIncrementDto semanticVersionIncrementDto)
    {
        _builder.SetBuildMetadata(currentVersion.BuildMetadata);
        _builder.SetPrefix(currentVersion.Prefix);
        SetIncrementedCoreVersion(currentVersion, semanticVersionIncrementDto.SemanticVersionCorePart);

        return _builder.BuildSemanticVersion();
    }

    private static bool IsNewPreReleaseIdentifier(SemanticVersion currentVersion, SemanticVersionIncrementDto semanticVersionIncrementDto)
    {
        return !string.IsNullOrWhiteSpace(semanticVersionIncrementDto.PreReleaseIdentifier)
            && currentVersion.PreReleaseIdentifier?.LastOrDefault() != semanticVersionIncrementDto.PreReleaseIdentifier;
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
}