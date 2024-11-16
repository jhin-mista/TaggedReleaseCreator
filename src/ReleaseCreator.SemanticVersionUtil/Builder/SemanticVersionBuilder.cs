using ReleaseCreator.SemanticVersionUtil.Types;

namespace ReleaseCreator.SemanticVersionUtil.Builder;

internal class SemanticVersionBuilder : ISemanticVersionBuilder
{
    private string? _prefix;

    private uint _major;
    private uint _minor;
    private uint _patch;

    private IList<string>? _buildMetadata;

    private IList<string>? _preReleaseIdentifier;
    private uint? _preReleaseNumber;

    public SemanticVersion GetSemanticVersion()
    {
        return new(_major, _minor, _patch, _preReleaseIdentifier, _preReleaseNumber, _buildMetadata, _prefix);
    }

    public void SetPrefix(string? prefix)
    {
        _prefix = prefix;
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