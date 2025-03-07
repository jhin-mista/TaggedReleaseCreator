﻿using Moq;
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

    [Test]
    public void Increment_WhenCalledForPreReleaseToPreReleaseIncrement_ShouldCallDirectorAsExpected()
    {
        //arrange
        var currentVersion = new SemanticVersion(1, 1, 1, new[] { "alpha" }, 1, []);
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

    [Test]
    public void Increment_WhenCalledForPreReleaseToStableIncrement_ShouldCallDirectorAsExpected()
    {
        //arrange
        var currentVersion = new SemanticVersion(1, 1, 1, new[] { "alpha" }, 1, []);
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