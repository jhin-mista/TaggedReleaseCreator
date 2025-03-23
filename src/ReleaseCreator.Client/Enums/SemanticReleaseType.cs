namespace ReleaseCreator.Client.Enums;

/// <summary>
/// Represents a semantic release type.
/// </summary>
public enum SemanticReleaseType
{
    /// <summary>
    /// The major version.
    /// </summary>
    Major = 1,

    /// <summary>
    /// The minor version.
    /// </summary>
    Minor = 2,

    /// <summary>
    /// The patch version.
    /// </summary>
    Patch = 3,
}