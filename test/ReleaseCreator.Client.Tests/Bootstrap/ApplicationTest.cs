using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Octokit;
using ReleaseCreator.Client.ReleaseCreation;
using ReleaseCreator.Client.Types;
using Application = ReleaseCreator.Client.Bootstrap.Application;

namespace ReleaseCreator.Client.Tests.Bootstrap;

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
        var releaseCreatorOptions = new ReleaseCreatorOptions("sha", Enums.ReleaseType.Patch, null, "access token");

        _releaseCreatorMock.Setup(x => x.CreateReleaseAsync(It.IsAny<ReleaseCreatorOptions>()))
            .ReturnsAsync(new Release());

        // act
        await _sut.RunAsync(releaseCreatorOptions);

        // assert
        _releaseCreatorMock.Verify(x => x.CreateReleaseAsync(releaseCreatorOptions), Times.Once);
        _releaseCreatorMock.VerifyNoOtherCalls();
    }

    [Test]
    public async Task RunAsync_WhenReleaseCreatorThrowsException_ShouldSetEnvironmentExitCode()
    {
        // arrange
        var releaseCreatorOptions = new ReleaseCreatorOptions("sha", Enums.ReleaseType.Patch, null, "access token");

        _releaseCreatorMock.Setup(x => x.CreateReleaseAsync(It.IsAny<ReleaseCreatorOptions>()))
            .ThrowsAsync(new Exception());

        // act
        await _sut.RunAsync(releaseCreatorOptions);

        // assert

        _releaseCreatorMock.Verify(x => x.CreateReleaseAsync(releaseCreatorOptions), Times.Once);
        _releaseCreatorMock.VerifyNoOtherCalls();

        Environment.ExitCode.Should().NotBe(0);
    }
}