using ReleaseCreator.SemanticVersionUtil.Types;

namespace ReleaseCreator.SemanticVersionUtil.Builder;

/// <summary>
/// Represents a builder for a <see cref="SemanticVersion"/>.
/// </summary>
public interface ISemanticVersionBuilder
{
    /// <summary>
    /// Sets an optional version prefix.
    /// </summary>
    /// <param name="prefix">The optional prefix.</param>
    public void SetPrefix(string? prefix);

    /// <summary>
    /// Sets the major version.
    /// </summary>
    /// <param name="majorVersion">The major version.</param>
    public void SetMajorVersion(uint majorVersion);

    /// <summary>
    /// Sets the minor version.
    /// </summary>
    /// <param name="minorVersion">The minor version.</param>
    public void SetMinorVersion(uint minorVersion);

    /// <summary>
    /// Sets the patch version.
    /// </summary>
    /// <param name="patchVersion">The patch version.</param>
    public void SetPatchVersion(uint patchVersion);

    /// <summary>
    /// Sets the pre-release number.
    /// </summary>
    /// <param name="preReleaseNumber">The pre-release number.</param>
    public void SetPreReleaseNumber(uint? preReleaseNumber);

    /// <summary>
    /// Sets the pre-release version (e.g. pre-release identifier + pre-release number).
    /// </summary>
    /// <param name="preReleaseVersion">The pre-release version.</param>
    public void SetPreReleaseVersion(string? preReleaseVersion);

    /// <summary>
    /// Sets the pre-release identifier.
    /// </summary>
    /// <param name="preReleaseIdentifier">The list of elements from a dot separated pre-release identifier.</param>
    public void SetPreReleaseIdentifier(IList<string> preReleaseIdentifier);

    /// <summary>
    /// Sets the pre-release identifier.
    /// </summary>
    /// <param name="preReleaseIdentifier">The dot separated pre-release identifier.</param>
    public void SetPreReleaseIdentifier(string? preReleaseIdentifier);

    /// <summary>
    /// Sets the build metadata.
    /// </summary>
    /// <param name="buildMetadata">The list of elements from a dot separated build metadata.</param>
    public void SetBuildMetadata(IList<string> buildMetadata);

    /// <summary>
    /// Sets the build metadata.
    /// </summary>
    /// <param name="buildMetadata"></param>
    public void SetBuildMetadata(string? buildMetadata);

    /// <summary>
    /// Builds a new <see cref="SemanticVersion"/>.
    /// </summary>
    public SemanticVersion BuildSemanticVersion();
}