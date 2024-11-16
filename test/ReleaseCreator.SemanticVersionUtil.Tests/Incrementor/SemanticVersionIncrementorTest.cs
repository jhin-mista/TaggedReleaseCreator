using Moq;
using ReleaseCreator.SemanticVersionUtil.Enums;
using ReleaseCreator.SemanticVersionUtil.Incrementor;
using ReleaseCreator.SemanticVersionUtil.Types;

namespace ReleaseCreator.SemanticVersionUtil.Tests.Incrementor;

[TestFixture]
public class SemanticVersionIncrementorTest
{
    private SemanticVersionIncrementor _sut;
    private Mock<ISemanticVersionIncrementDirector> _semanticVersionBuilderMock;

    [SetUp]
    public void SetUp()
    {
        _semanticVersionBuilderMock = new(MockBehavior.Strict);
        _sut = new(_semanticVersionBuilderMock.Object);
    }

    [Test]
    public void Increment_WhenCalledForPreReleaseToPreReleaseIncrement_ShouldCallDirectorAsExpected()
    {
        //arrange
        var currentVersion = new SemanticVersion(1, 1, 1, new[] { "alpha" }, 1, []);
        var incrementDto = new SemanticVersionIncrementDto(SemanticVersionCorePart.Major, null, true);

        _semanticVersionBuilderMock.Setup(x =>
            x.IncrementPreReleaseToPreRelease(It.IsAny<SemanticVersion>(), It.IsAny<SemanticVersionIncrementDto>()))
            .Returns(currentVersion);

        // act
        _sut.Increment(currentVersion, incrementDto);

        // assert
        _semanticVersionBuilderMock.Verify(x => x.IncrementPreReleaseToPreRelease(currentVersion, incrementDto), Times.Once);
        _semanticVersionBuilderMock.VerifyNoOtherCalls();
    }

    [Test]
    public void Increment_WhenCalledForPreReleaseToStableIncrement_ShouldCallDirectorAsExpected()
    {
        //arrange
        var currentVersion = new SemanticVersion(1, 1, 1, new[] { "alpha" }, 1, []);
        var incrementDto = new SemanticVersionIncrementDto(SemanticVersionCorePart.Major, null, false);

        _semanticVersionBuilderMock.Setup(x =>
            x.IncrementPreReleaseToStable(It.IsAny<SemanticVersion>(), It.IsAny<SemanticVersionIncrementDto>()))
            .Returns(currentVersion);

        // act
        _sut.Increment(currentVersion, incrementDto);

        // assert
        _semanticVersionBuilderMock.Verify(x => x.IncrementPreReleaseToStable(currentVersion, incrementDto), Times.Once);
        _semanticVersionBuilderMock.VerifyNoOtherCalls();
    }

    [Test]
    public void Increment_WhenCalledForStableToPreReleaseIncrement_ShouldCallDirectorAsExpected()
    {
        //arrange
        var currentVersion = new SemanticVersion(1, 1, 1, [], null, []);
        var incrementDto = new SemanticVersionIncrementDto(SemanticVersionCorePart.Major, "alpha", true);

        _semanticVersionBuilderMock.Setup(x =>
            x.IncrementStableToPreRelease(It.IsAny<SemanticVersion>(), It.IsAny<SemanticVersionIncrementDto>()))
            .Returns(currentVersion);

        // act
        _sut.Increment(currentVersion, incrementDto);

        // assert
        _semanticVersionBuilderMock.Verify(x => x.IncrementStableToPreRelease(currentVersion, incrementDto), Times.Once);
        _semanticVersionBuilderMock.VerifyNoOtherCalls();
    }

    [Test]
    public void Increment_WhenCalledForStableToStableIncrement_ShouldCallDirectorAsExpected()
    {
        //arrange
        var currentVersion = new SemanticVersion(1, 1, 1, [], null, []);
        var incrementDto = new SemanticVersionIncrementDto(SemanticVersionCorePart.Major, null, false);

        _semanticVersionBuilderMock.Setup(x =>
            x.IncrementStableToStable(It.IsAny<SemanticVersion>(), It.IsAny<SemanticVersionIncrementDto>()))
            .Returns(currentVersion);

        // act
        _sut.Increment(currentVersion, incrementDto);

        // assert
        _semanticVersionBuilderMock.Verify(x => x.IncrementStableToStable(currentVersion, incrementDto), Times.Once);
        _semanticVersionBuilderMock.VerifyNoOtherCalls();
    }
}