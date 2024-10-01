using ReleaseCreator.CommandLine.Enums;
using ReleaseCreator.CommandLine.Types;
using ReleaseCreator.Git.Tag;
using ReleaseCreator.SemanticVersionUtil.Enums;
using ReleaseCreator.SemanticVersionUtil.Incrementor;
using ReleaseCreator.SemanticVersionUtil.Parser;
using ReleaseCreator.SemanticVersionUtil.Types;

namespace ReleaseCreator.CommandLine.VersionCalculation
{
    internal class NextVersionCalculator : INextVersionCalculator
    {
        private readonly ISemanticVersionIncrementor _semanticVersionIncrementor;
        private readonly ITagRetriever _tagRetriever;
        private readonly ISemanticVersionParser _semanticVersionParser;

        internal NextVersionCalculator(
            ITagRetriever tagRetriever,
            ISemanticVersionIncrementor semanticVersionIncrementor,
            ISemanticVersionParser semanticVersionParser)
        {
            _tagRetriever = tagRetriever;
            _semanticVersionIncrementor = semanticVersionIncrementor;
            _semanticVersionParser = semanticVersionParser;
        }

        public SemanticVersion CalculateNextVersion(ReleaseCreatorOptions releaseCreatorOptions)
        {
            var currentSemanticVersion = GetCurrentSemanticVersion();
            var coreVersionPartToUpdate = GetSemanticVersionCorePart(releaseCreatorOptions.VersionIncreasePart);
            var versionIncrementDto = new SemanticVersionIncrementDto(
                coreVersionPartToUpdate,
                releaseCreatorOptions.PreReleaseIdentifier,
                releaseCreatorOptions.IsPreRelease);

            return _semanticVersionIncrementor.Increment(currentSemanticVersion, versionIncrementDto);
        }

        private SemanticVersion GetCurrentSemanticVersion()
        {
            var currentTag = _tagRetriever.GetLatestTag();

            return currentTag == null
                ? new SemanticVersion(0, 0, 0, null, null, null)
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
}