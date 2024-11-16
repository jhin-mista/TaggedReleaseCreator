using ReleaseCreator.SemanticVersionUtil.Builder;
using ReleaseCreator.SemanticVersionUtil.Types;
using System.Text.RegularExpressions;

namespace ReleaseCreator.SemanticVersionUtil.Parser;

/// <summary>
/// Provides methods for parsing a semantic version into a <see cref="SemanticVersion"/>.
/// </summary>
/// <remarks>A leading v (case insensitive) character can be handled (e.g. <c>v1.2.3</c> like it's commonly found in git version tags.)</remarks>
/// <param name="semanticVersionBuilder">The builder for creating a <see cref="SemanticVersion"/>.</param>
public partial class SemanticVersionParser(ISemanticVersionBuilder semanticVersionBuilder) : ISemanticVersionParser
{
    private const string _major = "major";
    private const string _minor = "minor";
    private const string _patch = "patch";
    private const string _preRelease = "preRelease";
    private const string _buildMetadata = "buildMetadata";
    private const string _prefix = "prefix";
    private const string _regex = $@"^(?<{_prefix}>[v|V]?)(?<{_major}>0|[1-9]\d*)\.(?<{_minor}>0|[1-9]\d*)\.(?<{_patch}>0|[1-9]\d*)(?:-(?<{_preRelease}>(?:0|[1-9]\d*|\d*[a-zA-Z-][0-9a-zA-Z-]*)(?:\.(?:0|[1-9]\d*|\d*[a-zA-Z-][0-9a-zA-Z-]*))*))?(?:\+(?<{_buildMetadata}>[0-9a-zA-Z-]+(?:\.[0-9a-zA-Z-]+)*))?$";

    private readonly ISemanticVersionBuilder _semanticVersionBuilder = semanticVersionBuilder;

    /// <inheritdoc/>
    /// <exception cref="FormatException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    public SemanticVersion Parse(string semanticVersionString)
    {
        ArgumentException.ThrowIfNullOrEmpty(semanticVersionString, nameof(semanticVersionString));

        var match = SemanticVersionRegex().Match(semanticVersionString);

        if (match.Success)
        {
            var groups = match.Groups;
            _semanticVersionBuilder.SetMajorVersion(uint.Parse(groups[_major].Value));
            _semanticVersionBuilder.SetMinorVersion(uint.Parse(groups[_minor].Value));
            _semanticVersionBuilder.SetPatchVersion(uint.Parse(groups[_patch].Value));

            _semanticVersionBuilder.SetPreReleaseVersion(groups[_preRelease].Value);

            _semanticVersionBuilder.SetBuildMetadata(groups[_buildMetadata].Value);

            _semanticVersionBuilder.SetPrefix(groups[_prefix].Value);

            return _semanticVersionBuilder.BuildSemanticVersion();
        }

        throw new FormatException($"'{semanticVersionString}' is not a valid semantic version.");
    }

    [GeneratedRegex(_regex, RegexOptions.None, 5_000)]
    private static partial Regex SemanticVersionRegex();
}