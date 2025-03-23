using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Octokit;
using ReleaseCreator.Client.ReleaseCreation;
using ReleaseCreator.Client.Types;
using ReleaseCreator.CommandLine.Types;
using Application = ReleaseCreator.CommandLine.Bootstrap.Application;
using ReleaseCreatorSemanticReleaseType = ReleaseCreator.Client.Enums.SemanticReleaseType;
using SemanticReleaseType = ReleaseCreator.CommandLine.Enums.SemanticReleaseType;

namespace ReleaseCreator.CommandLine.Tests.Bootstrap;

[TestFixture]
public class ApplicationTest
{
    private Application _sut;
    private Mock<ILogger<Application>> _loggerMock;
    private Mock<IReleaseCreator> _releaseCreatorMock;

    [SetUp]
    public void SetUp()
    {
        _loggerMock = new(MockBehavior.Loose);
        _releaseCreatorMock = new(MockBehavior.Strict);

        _sut = new(_loggerMock.Object, _releaseCreatorMock.Object);
    }

    [Test]
    public async Task RunAsync_ShouldCallReleaseCreator()
    {
        // arrange

        var releaseCreatorCommandLineOptions = new ReleaseCreatorCommandLineOptions("commitish", SemanticReleaseType.Patch, "alpha", "access token");

        ReleaseCreatorOptions? actualReleaseCreatorOptions = null;
        _releaseCreatorMock.Setup(x => x.CreateReleaseAsync(It.IsAny<ReleaseCreatorOptions>()))
            .Callback<ReleaseCreatorOptions>(x => actualReleaseCreatorOptions = x)
            .ReturnsAsync(new Release());

        // act
        await _sut.RunAsync(releaseCreatorCommandLineOptions);

        // assert
        _releaseCreatorMock.Verify(x => x.CreateReleaseAsync(It.IsAny<ReleaseCreatorOptions>()), Times.Once);
        actualReleaseCreatorOptions.Should().NotBeNull();
        actualReleaseCreatorOptions!.Commitish.Should().Be(releaseCreatorCommandLineOptions.Commitish);
        actualReleaseCreatorOptions.SemanticReleaseType.Should().Be(ReleaseCreatorSemanticReleaseType.Patch);
        actualReleaseCreatorOptions.PreReleaseIdentifier.Should().Be(releaseCreatorCommandLineOptions.PreReleaseIdentifier);
    }

    [Test]
    public async Task RunAsync_WhenReleaseCreatorThrowsException_ShouldSetEnvironmentExitCode()
    {
        // arrange
        var releaseCreatorOptions = new ReleaseCreatorCommandLineOptions("sha", SemanticReleaseType.Patch, null, "access token");

        _releaseCreatorMock.Setup(x => x.CreateReleaseAsync(It.IsAny<ReleaseCreatorOptions>()))
            .ThrowsAsync(new Exception());

        // act
        await _sut.RunAsync(releaseCreatorOptions);

        // assert

        _releaseCreatorMock.Verify(x => x.CreateReleaseAsync(It.IsAny<ReleaseCreatorOptions>()), Times.Once);
        _releaseCreatorMock.VerifyNoOtherCalls();

        Environment.ExitCode.Should().NotBe(0);
    }
}