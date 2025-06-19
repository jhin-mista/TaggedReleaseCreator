using Moq;
using ReleaseCreator.SemanticVersionUtil.Enums;
using ReleaseCreator.SemanticVersionUtil.Incrementor;
using ReleaseCreator.SemanticVersionUtil.Types;

namespace ReleaseCreator.SemanticVersionUtil.Tests.Incrementor;

[TestFixture]
public class SemanticVersionIncrementorTest
{
    private SemanticVersionIncrementor _sut;
    private Mock<ISemanticVersionIncrementDirector> _semanticVersionIncrementDirectorMock;

    [SetUp]
    public void SetUp()
    {
        _semanticVersionIncrementDirectorMock = new(MockBehavior.Strict);
        _sut = new(_semanticVersionIncrementDirectorMock.Object);
    }

    public static IEnumerable<TestCaseData> PreReleaseSemanticVersions
    {
        get
        {
            yield return new(new string[] { "alpha" }, 1u);
            yield return new(new string[] { "alpha" }, null);
            yield return new(Array.Empty<string>(), 1u);
            yield return new(new string[] { string.Empty }, 1u);
            yield return new(new string[] { string.Empty }, null);
        }
    }

    [TestCaseSource(nameof(PreReleaseSemanticVersions))]
    public void Increment_WhenCalledForPreReleaseToPreReleaseIncrement_ShouldCallDirectorAsExpected(string[] preReleaseIdentifier, uint? preReleaseNumber)
    {
        //arrange
        var currentVersion = new SemanticVersion(1, 1, 1, preReleaseIdentifier, preReleaseNumber, []);
        var incrementDto = new SemanticVersionIncrementDto(SemanticVersionCorePart.Major, string.Empty);

        _semanticVersionIncrementDirectorMock.Setup(x =>
            x.IncrementPreReleaseToPreRelease(It.IsAny<SemanticVersion>(), It.IsAny<SemanticVersionIncrementDto>()))
            .Returns(currentVersion);

        // act
        _sut.Increment(currentVersion, incrementDto);

        // assert
        _semanticVersionIncrementDirectorMock.Verify(x => x.IncrementPreReleaseToPreRelease(currentVersion, incrementDto), Times.Once);
        _semanticVersionIncrementDirectorMock.VerifyNoOtherCalls();
    }

    [TestCaseSource(nameof(PreReleaseSemanticVersions))]
    public void Increment_WhenCalledForPreReleaseToStableIncrement_ShouldCallDirectorAsExpected(string[] preReleaseIdentifier, uint? preReleaseNumber)
    {
        //arrange
        var currentVersion = new SemanticVersion(1, 1, 1, preReleaseIdentifier, preReleaseNumber, []);
        var incrementDto = new SemanticVersionIncrementDto(SemanticVersionCorePart.Major, null);

        _semanticVersionIncrementDirectorMock.Setup(x =>
            x.IncrementPreReleaseToStable(It.IsAny<SemanticVersion>(), It.IsAny<SemanticVersionIncrementDto>()))
            .Returns(currentVersion);

        // act
        _sut.Increment(currentVersion, incrementDto);

        // assert
        _semanticVersionIncrementDirectorMock.Verify(x => x.IncrementPreReleaseToStable(currentVersion, incrementDto), Times.Once);
        _semanticVersionIncrementDirectorMock.VerifyNoOtherCalls();
    }

    [Test]
    public void Increment_WhenCalledForStableToPreReleaseIncrement_ShouldCallDirectorAsExpected()
    {
        //arrange
        var currentVersion = new SemanticVersion(1, 1, 1, [], null, []);
        var incrementDto = new SemanticVersionIncrementDto(SemanticVersionCorePart.Major, "alpha");

        _semanticVersionIncrementDirectorMock.Setup(x =>
            x.IncrementStableToPreRelease(It.IsAny<SemanticVersion>(), It.IsAny<SemanticVersionIncrementDto>()))
            .Returns(currentVersion);

        // act
        _sut.Increment(currentVersion, incrementDto);

        // assert
        _semanticVersionIncrementDirectorMock.Verify(x => x.IncrementStableToPreRelease(currentVersion, incrementDto), Times.Once);
        _semanticVersionIncrementDirectorMock.VerifyNoOtherCalls();
    }

    [Test]
    public void Increment_WhenCalledForStableToStableIncrement_ShouldCallDirectorAsExpected()
    {
        //arrange
        var currentVersion = new SemanticVersion(1, 1, 1, [], null, []);
        var incrementDto = new SemanticVersionIncrementDto(SemanticVersionCorePart.Major, null);

        _semanticVersionIncrementDirectorMock.Setup(x =>
            x.IncrementStableToStable(It.IsAny<SemanticVersion>(), It.IsAny<SemanticVersionIncrementDto>()))
            .Returns(currentVersion);

        // act
        _sut.Increment(currentVersion, incrementDto);

        // assert
        _semanticVersionIncrementDirectorMock.Verify(x => x.IncrementStableToStable(currentVersion, incrementDto), Times.Once);
        _semanticVersionIncrementDirectorMock.VerifyNoOtherCalls();
    }
}