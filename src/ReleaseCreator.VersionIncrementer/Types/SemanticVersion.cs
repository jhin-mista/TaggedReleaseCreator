﻿namespace ReleaseCreator.VersionIncrementor.Types;

/// <summary>
/// Represents a semantic version.
/// </summary>
/// <param name="Major">The major version.</param>
/// <param name="Minor">The minor version.</param>
/// <param name="Patch">The patch version.</param>
/// <param name="PreReleaseIdentifier">The optional pre-release identifier.</param>
/// <param name="PreReleaseNumber">The optional pre-release number.</param>
/// <param name="BuildMetadata">The optional build metadata.</param>
public record SemanticVersion(uint Major, uint Minor, uint Patch, IList<string>? PreReleaseIdentifier, uint? PreReleaseNumber, IList<string>? BuildMetadata)
{
    private static readonly char _separator = '.';
    private static readonly char _preReleaseStartSign = '-';
    private static readonly char _buildMetadataStartSign = '+';

    /// <inheritdoc/>
    public override string ToString()
    {
        var coreVersion = string.Join(_separator, Major, Minor, Patch);

        var preReleaseVersion = GetPreReleaseVersion();

        var buildMetaData = BuildMetadata != null
            ? _buildMetadataStartSign + string.Join(_separator, BuildMetadata)
            : string.Empty;

        return coreVersion + preReleaseVersion + buildMetaData;
    }

    private string GetPreReleaseVersion()
    {
        var preReleaseIdentifier = PreReleaseIdentifier != null
            ? string.Join(_separator, PreReleaseIdentifier)
            : null;

        string preReleaseVersion;

        if (preReleaseIdentifier != null && PreReleaseNumber.HasValue)
        {
            preReleaseVersion = _preReleaseStartSign + string.Join(_separator, preReleaseIdentifier, PreReleaseNumber);
        }
        else if (preReleaseIdentifier == null && PreReleaseNumber.HasValue)
        {
            preReleaseVersion = $"{_preReleaseStartSign}{PreReleaseNumber.Value}";
        }
        else if (preReleaseIdentifier != null && !PreReleaseNumber.HasValue)
        {
            preReleaseVersion = _preReleaseStartSign + preReleaseIdentifier;
        }
        else
        {
            preReleaseVersion = string.Empty;
        }

        return preReleaseVersion;
    }
}