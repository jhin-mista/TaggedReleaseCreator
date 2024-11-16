namespace ReleaseCreator.CommandLine.Enums;

/// <summary>
/// Represents a semantic version core part.
/// </summary>
public enum ReleaseType
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