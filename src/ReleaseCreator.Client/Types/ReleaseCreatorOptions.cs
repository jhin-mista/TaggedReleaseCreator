﻿using ReleaseCreator.Client.Enums;
using System.Text;

namespace ReleaseCreator.Client.Types;

/// <summary>
/// Holds options for creating a release, including versioning and access details.
/// </summary>
/// <param name="Commitish">Specifies the unique identifier for the commit associated with the release.</param>
/// <param name="SemanticReleaseType">Indicates the type of semantic release.</param>
/// <param name="PreReleaseIdentifier">Allows for the inclusion of an identifier for pre-release versions.</param>
public record ReleaseCreatorOptions(
    string Commitish,
    SemanticReleaseType SemanticReleaseType,
    string? PreReleaseIdentifier)
{
    internal bool IsPreRelease => PreReleaseIdentifier != null;

    /// <summary>
    /// Appends member information to the provided <paramref name="builder"/>.
    /// </summary>
    /// <param name="builder">The string builder used to accumulate formatted member data.</param>
    protected virtual bool PrintMembers(StringBuilder builder)
    {
        builder.Append($"{nameof(Commitish)} = {Commitish}, {nameof(SemanticReleaseType)} = {SemanticReleaseType}, {nameof(PreReleaseIdentifier)} = {PreReleaseIdentifier}, {nameof(IsPreRelease)} = {IsPreRelease}");

        return true;
    }
}