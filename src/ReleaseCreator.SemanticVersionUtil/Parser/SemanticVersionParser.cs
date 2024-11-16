using ReleaseCreator.SemanticVersionUtil.Types;
using System.Text.RegularExpressions;

namespace ReleaseCreator.SemanticVersionUtil.Parser
{
    /// <summary>
    /// Provides methods for parsing a semantic version into a <see cref="SemanticVersion"/>.
    /// </summary>
    /// <remarks>A leading v (case insensitive) character can be handled (e.g. <c>v1.2.3</c> like it's commonly found in git version tags.)</remarks>
    public partial class SemanticVersionParser : ISemanticVersionParser
    {
        private const string _major = "major";
        private const string _minor = "minor";
        private const string _patch = "patch";
        private const string _preRelease = "preRelease";
        private const string _buildMetadata = "buildMetadata";
        private const string _prefix = "prefix";
        private const string _regex = $@"^(?<{_prefix}>[v|V]?)(?<{_major}>0|[1-9]\d*)\.(?<{_minor}>0|[1-9]\d*)\.(?<{_patch}>0|[1-9]\d*)(?:-(?<{_preRelease}>(?:0|[1-9]\d*|\d*[a-zA-Z-][0-9a-zA-Z-]*)(?:\.(?:0|[1-9]\d*|\d*[a-zA-Z-][0-9a-zA-Z-]*))*))?(?:\+(?<{_buildMetadata}>[0-9a-zA-Z-]+(?:\.[0-9a-zA-Z-]+)*))?$";

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
                var major = uint.Parse(groups[_major].Value);
                var minor = uint.Parse(groups[_minor].Value);
                var patch = uint.Parse(groups[_patch].Value);

                var (preReleaseIdentifier, preReleaseNumber) = GetParsedPreReleaseVersion(groups[_preRelease].Value);

                var buildMetadata = GetParsedBuildMetadata(groups[_buildMetadata].Value);

                var prefix = GetParsedPrefix(groups[_prefix].Value);

                return new(major, minor, patch, preReleaseIdentifier, preReleaseNumber, buildMetadata, prefix);
            }

            throw new FormatException($"'{semanticVersionString}' is not a valid semantic version.");
        }

        [GeneratedRegex(_regex, RegexOptions.None, 5_000)]
        private static partial Regex SemanticVersionRegex();

        private static (IList<string>?, uint?) GetParsedPreReleaseVersion(string? preReleaseVersion)
        {
            if (string.IsNullOrWhiteSpace(preReleaseVersion))
            {
                return (null, null);
            }

            var parts = preReleaseVersion.Split('.');

            uint? preReleaseNumber = uint.TryParse(parts[^1], out var parseResult) ? parseResult : null;
            IList<string>? preReleaseIdentifier;

            if (preReleaseNumber == null)
            {
                preReleaseIdentifier = [.. parts];
            }
            else
            {
                preReleaseIdentifier = parts.Length > 1 ? parts.Take(parts.Length - 1).ToList() : null;
            }

            return (preReleaseIdentifier, preReleaseNumber);
        }

        private static IList<string>? GetParsedBuildMetadata(string? buildMetadata)
        {
            if (string.IsNullOrWhiteSpace(buildMetadata))
            {
                return null;
            }

            return [.. buildMetadata.Split(".")];
        }

        private static string? GetParsedPrefix(string prefix)
        {
            return prefix.Length > 0
                ? prefix
                : null;
        }
    }
}