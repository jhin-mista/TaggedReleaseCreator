using ReleaseCreator.SemanticVersionUtil.Types;
using System.Text.RegularExpressions;

namespace ReleaseCreator.SemanticVersionUtil.Builder;

/// <inheritdoc cref="ISemanticVersionBuilder"/>
internal partial class SemanticVersionBuilder : ISemanticVersionBuilder
{
    private const char DotSeparator = '.';

    private string? _prefix;

    private uint _major;
    private uint _minor;
    private uint _patch;

    private IList<string> _preReleaseIdentifier = [];
    private uint? _preReleaseNumber;

    private IList<string> _buildMetadata = [];

    /// <inheritdoc/>
    public SemanticVersion BuildSemanticVersion()
    {
        return new(_major, _minor, _patch, _preReleaseIdentifier, _preReleaseNumber, _buildMetadata, _prefix);
    }

    /// <inheritdoc/>
    public void SetPrefix(string? prefix)
    {
        if (string.IsNullOrWhiteSpace(prefix))
        {
            _prefix = null;
        }
        else
        {
            _prefix = prefix.Trim();
        }
    }

    /// <inheritdoc/>
    public void SetBuildMetadata(IList<string> buildMetadata)
    {
        ValidateBuildMetadata(string.Join(DotSeparator, buildMetadata));

        _buildMetadata = buildMetadata;
    }

    /// <inheritdoc/>
    public void SetBuildMetadata(string? buildMetadata)
    {
        if (string.IsNullOrWhiteSpace(buildMetadata))
        {
            _buildMetadata = [];

            return;
        }

        ValidateBuildMetadata(buildMetadata);

        _buildMetadata = SplitBySeparator(buildMetadata);
    }

    /// <inheritdoc/>
    public void SetMajorVersion(uint majorVersion)
    {
        _major = majorVersion;
    }

    /// <inheritdoc/>
    public void SetMinorVersion(uint minorVersion)
    {
        _minor = minorVersion;
    }

    /// <inheritdoc/>
    public void SetPatchVersion(uint patchVersion)
    {
        _patch = patchVersion;
    }

    /// <inheritdoc/>
    public void SetPreReleaseVersion(string? preReleaseVersion)
    {
        if (string.IsNullOrWhiteSpace(preReleaseVersion))
        {
            _preReleaseIdentifier = [];
            _preReleaseNumber = null;

            return;
        }

        ValidatePreReleaseVersion(preReleaseVersion);

        var (preReleaseNumber, preReleaseIdentifier) = SplitPreReleaseNumberAndIdentifier(preReleaseVersion);

        _preReleaseIdentifier = preReleaseIdentifier;
        _preReleaseNumber = preReleaseNumber;
    }

    /// <inheritdoc/>
    public void SetPreReleaseIdentifier(IList<string> preReleaseIdentifier)
    {
        ValidatePreReleaseVersion(string.Join(DotSeparator, preReleaseIdentifier));

        _preReleaseIdentifier = preReleaseIdentifier;
    }

    /// <inheritdoc/>
    public void SetPreReleaseIdentifier(string? preReleaseIdentifier)
    {
        if (string.IsNullOrWhiteSpace(preReleaseIdentifier))
        {
            _preReleaseIdentifier = [];

            return;
        }

        ValidatePreReleaseVersion(preReleaseIdentifier);

        _preReleaseIdentifier = SplitBySeparator(preReleaseIdentifier);
    }

    /// <inheritdoc/>
    public void SetPreReleaseNumber(uint? preReleaseNumber)
    {
        _preReleaseNumber = preReleaseNumber;
    }

    private static (uint?, IList<string>) SplitPreReleaseNumberAndIdentifier(string preReleaseVersion)
    {
        var parts = SplitBySeparator(preReleaseVersion);

        if (parts.Length <= 0)
        {
            return (null, []);
        }

        uint? preReleaseNumber = uint.TryParse(parts[^1], out var parseResult) ? parseResult : null;

        IList<string> preReleaseIdentifier;
        if (preReleaseNumber == null)
        {
            preReleaseIdentifier = parts;
        }
        else
        {
            preReleaseIdentifier = parts.Length > 1 ? [.. parts.Take(parts.Length - 1)] : [];
        }

        return (preReleaseNumber, preReleaseIdentifier);
    }

    private static string[] SplitBySeparator(string input)
    {
        return input.Split(DotSeparator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    }

    [GeneratedRegex(@"(?:0|[1-9]\d*|\d*[a-zA-Z-][0-9a-zA-Z-]*)(?:\.(?:0|[1-9]\d*|\d*[a-zA-Z-][0-9a-zA-Z-]*))*", RegexOptions.None, 5_000)]
    private static partial Regex PreReleaseIdentifierRegex();

    private static void ValidatePreReleaseVersion(string preReleaseVersion)
    {
        var match = PreReleaseIdentifierRegex().Match(preReleaseVersion);

        if (!string.IsNullOrWhiteSpace(preReleaseVersion) && !match.Success)
        {
            throw new FormatException($"'{preReleaseVersion}' is not a valid pre-release version");
        }
    }

    [GeneratedRegex(@"([0-9a-zA-Z-]+(?:\.[0-9a-zA-Z-]+)*)?", RegexOptions.None, 5_000)]
    private static partial Regex BuildMetadataRegex();

    private static void ValidateBuildMetadata(string buildMetadata)
    {
        var match = BuildMetadataRegex().Match(buildMetadata);

        if (!match.Success)
        {
            throw new FormatException($"'{buildMetadata}' is not a valid build identifier");
        }
    }
}