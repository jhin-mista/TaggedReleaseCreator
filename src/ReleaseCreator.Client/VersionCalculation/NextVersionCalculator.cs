﻿using ReleaseCreator.Client.Enums;
using ReleaseCreator.Client.Types;
using ReleaseCreator.Git.Tag;
using ReleaseCreator.SemanticVersionUtil.Enums;
using ReleaseCreator.SemanticVersionUtil.Incrementor;
using ReleaseCreator.SemanticVersionUtil.Parser;
using ReleaseCreator.SemanticVersionUtil.Types;

namespace ReleaseCreator.Client.VersionCalculation;

internal class NextVersionCalculator(
    ITagRetriever tagRetriever,
    ISemanticVersionIncrementor semanticVersionIncrementor,
    ISemanticVersionParser semanticVersionParser) : INextVersionCalculator
{
    private readonly ISemanticVersionIncrementor _semanticVersionIncrementor = semanticVersionIncrementor;
    private readonly ITagRetriever _tagRetriever = tagRetriever;
    private readonly ISemanticVersionParser _semanticVersionParser = semanticVersionParser;

    public SemanticVersion CalculateNextVersion(ReleaseCreatorOptions releaseCreatorOptions)
    {
        var currentSemanticVersion = GetCurrentSemanticVersion();
        var coreVersionPartToUpdate = GetSemanticVersionCorePart(releaseCreatorOptions.VersionIncreasePart);
        var versionIncrementDto = new SemanticVersionIncrementDto(
            coreVersionPartToUpdate,
            releaseCreatorOptions.PreReleaseIdentifier);

        return _semanticVersionIncrementor.Increment(currentSemanticVersion, versionIncrementDto);
    }

    private SemanticVersion GetCurrentSemanticVersion()
    {
        var currentTag = _tagRetriever.GetLatestTag();

        return currentTag == null
            ? new SemanticVersion(0, 0, 0, [], null, [])
            : _semanticVersionParser.Parse(currentTag);
    }

    private static SemanticVersionCorePart GetSemanticVersionCorePart(ReleaseType releaseType)
    {
        return releaseType switch
        {
            ReleaseType.Major => SemanticVersionCorePart.Major,
            ReleaseType.Minor => SemanticVersionCorePart.Minor,
            ReleaseType.Patch => SemanticVersionCorePart.Patch,
            _ => throw new NotSupportedException($"{nameof(ReleaseType)} {releaseType} cannot be mapped to a {nameof(SemanticVersionCorePart)}")
        };
    }
}