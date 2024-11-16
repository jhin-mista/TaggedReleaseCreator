using FluentAssertions;
using Moq;
using ReleaseCreator.CommandLine.Enums;
using ReleaseCreator.CommandLine.Types;
using ReleaseCreator.Git.Tag;
using ReleaseCreator.SemanticVersionUtil.Enums;
using ReleaseCreator.SemanticVersionUtil.Incrementor;
using ReleaseCreator.SemanticVersionUtil.Parser;
using ReleaseCreator.SemanticVersionUtil.Types;

namespace ReleaseCreator.CommandLine.Tests
{
    [TestFixture]
    public class NextVersionCalculatorTest
    {
        private NextVersionCalculator _sut;
        private Mock<ISemanticVersionIncrementor> _semanticVersionIncrementorMock;
        private Mock<ITagRetriever> _tagRetrieverMock;
        private Mock<ISemanticVersionParser> _semanticVersionParserMock;

        [SetUp]
        public void SetUp()
        {
            _semanticVersionIncrementorMock = new(MockBehavior.Strict);
            _tagRetrieverMock = new(MockBehavior.Strict);
            _semanticVersionParserMock = new(MockBehavior.Strict);

            _sut = new(_tagRetrieverMock.Object, _semanticVersionIncrementorMock.Object, _semanticVersionParserMock.Object);
        }

        [Test]
        public void CalculateNextVersion_ShouldCallDependenciesAsExpected()
        {
            // arrange
            const string RetrievedTag = "0.0.0";
            var input = new ReleaseCreatorOptions("branch name", ReleaseType.Major, null, false, "access token");
            var returnedIncrementedVersion = new SemanticVersion(1, 1, 1, null, null, null);
            var returnedCurrentVersion = new SemanticVersion(0, 0, 0, null, null, null);

            SemanticVersionIncrementDto? actualSemanticVersionIncrementDto = null;
            _semanticVersionIncrementorMock.Setup(x => x.Increment(It.IsAny<SemanticVersion>(), It.IsAny<SemanticVersionIncrementDto>()))
                .Callback<SemanticVersion, SemanticVersionIncrementDto>((_, incrementDto) => actualSemanticVersionIncrementDto = incrementDto)
                .Returns(returnedIncrementedVersion);

            _tagRetrieverMock.Setup(x => x.GetLatestTag()).Returns(RetrievedTag);

            _semanticVersionParserMock.Setup(x => x.Parse(It.IsAny<string>()))
                .Returns(returnedCurrentVersion);

            // act
            var result = _sut.CalculateNextVersion(input);

            // assert
            result.Should().Be(returnedIncrementedVersion);

            _semanticVersionIncrementorMock.Verify(x => x.Increment(returnedCurrentVersion, It.IsAny<SemanticVersionIncrementDto>()), Times.Once);
            _semanticVersionIncrementorMock.VerifyNoOtherCalls();
            actualSemanticVersionIncrementDto.Should().NotBeNull();
            actualSemanticVersionIncrementDto!.IsPreRelease.Should().Be(input.IsPreRelease);
            actualSemanticVersionIncrementDto.PreReleaseIdentifier.Should().Be(input.PreReleaseIdentifier);

            _tagRetrieverMock.Verify(x => x.GetLatestTag(), Times.Once);
            _tagRetrieverMock.VerifyNoOtherCalls();

            _semanticVersionParserMock.Verify(x => x.Parse(RetrievedTag), Times.Once);
            _semanticVersionParserMock.VerifyNoOtherCalls();
        }

        [Test]
        public void CalculateNextVersion_WhenNoTagExists_ShouldSetCurrentVersionAsZero()
        {
            // arrange
            var input = new ReleaseCreatorOptions("branch name", ReleaseType.Major, null, false, "access token");
            var expectedCurrentVersion = new SemanticVersion(0, 0, 0, null, null, null);

            SemanticVersion? actualCurrentSemanticVersion = null;
            _semanticVersionIncrementorMock.Setup(x => x.Increment(It.IsAny<SemanticVersion>(), It.IsAny<SemanticVersionIncrementDto>()))
                .Callback<SemanticVersion, SemanticVersionIncrementDto>((currentVersion, _) => actualCurrentSemanticVersion = currentVersion)
                .Returns(new SemanticVersion(1, 1, 1, null, null, null));

            _tagRetrieverMock.Setup(x => x.GetLatestTag()).Returns((string?)null);

            // act
            _sut.CalculateNextVersion(input);

            // assert
            actualCurrentSemanticVersion.Should().BeEquivalentTo(expectedCurrentVersion);

            _semanticVersionParserMock.Verify(x => x.Parse(It.IsAny<string>()), Times.Never);
        }

        [TestCase(ReleaseType.Major, SemanticVersionCorePart.Major)]
        [TestCase(ReleaseType.Minor, SemanticVersionCorePart.Minor)]
        [TestCase(ReleaseType.Patch, SemanticVersionCorePart.Patch)]
        public void CalculateNextVersion_ShouldMapReleaseTypeToExpectedSemanticVersionCorePart(ReleaseType releaseType, SemanticVersionCorePart expectedSemanticVersionCorePart)
        {
            // arrange
            var input = new ReleaseCreatorOptions("branch name", releaseType, null, false, "access token");

            SemanticVersionIncrementDto? actualSemanticVersionIncrementDto = null;
            _semanticVersionIncrementorMock.Setup(x => x.Increment(It.IsAny<SemanticVersion>(), It.IsAny<SemanticVersionIncrementDto>()))
                .Callback<SemanticVersion, SemanticVersionIncrementDto>((_, incrementDto) => actualSemanticVersionIncrementDto = incrementDto)
                .Returns(new SemanticVersion(1, 1, 1, null, null, null));

            _tagRetrieverMock.Setup(x => x.GetLatestTag()).Returns("0.0.0");

            _semanticVersionParserMock.Setup(x => x.Parse(It.IsAny<string>()))
                .Returns(new SemanticVersion(0, 0, 0, null, null, null));

            // act
            _sut.CalculateNextVersion(input);

            // assert
            actualSemanticVersionIncrementDto.Should().NotBeNull();
            actualSemanticVersionIncrementDto!.SemanticVersionCorePart.Should().Be(expectedSemanticVersionCorePart);
        }

        [Test]
        public void CalculateNextVersion_WhenSuppliedWithInvalidReleaseType_ShouldThrowException()
        {
            // arrange
            var invalidReleaseType = (ReleaseType)(-1);
            var input = new ReleaseCreatorOptions("branch name", invalidReleaseType, null, false, "access token");

            _tagRetrieverMock.Setup(x => x.GetLatestTag()).Returns("0.0.0");

            _semanticVersionParserMock.Setup(x => x.Parse(It.IsAny<string>()))
                .Returns(new SemanticVersion(0, 0, 0, null, null, null));

            // act
            var invocation = _sut.Invoking(x => x.CalculateNextVersion(input));

            // assert
            invocation.Should().Throw<NotSupportedException>().WithMessage($"{nameof(ReleaseType)} {invalidReleaseType} cannot be mapped to a {nameof(SemanticVersionCorePart)}");

            _semanticVersionIncrementorMock.Verify(x => x.Increment(It.IsAny<SemanticVersion>(), It.IsAny<SemanticVersionIncrementDto>()), Times.Never);
        }
    }
}