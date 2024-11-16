using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Octokit;
using ReleaseCreator.Client.Enums;
using ReleaseCreator.Client.ReleaseCreation.GitHub;
using ReleaseCreator.Client.Types;
using ReleaseCreator.Client.Util;
using ReleaseCreator.Client.VersionCalculation;
using ReleaseCreator.SemanticVersionUtil.Types;

namespace ReleaseCreator.Client.Tests.ReleaseCreation.GitHub;

[TestFixture]
public class GitHubReleaseCreatorTest
{
    private GitHubReleaseCreator _sut;
    private Mock<IReleasesClient> _releasesClientMock;
    private Mock<INextVersionCalculator> _nextVersionCalculatorMock;
    private Mock<IEnvironmentService> _environmentServiceMock;
    private Mock<IFileService> _fileServiceMock;
    private Mock<ILogger<GitHubReleaseCreator>> _loggerMock;

    [SetUp]
    public void SetUp()
    {
        _releasesClientMock = new(MockBehavior.Strict);
        _nextVersionCalculatorMock = new(MockBehavior.Strict);
        _environmentServiceMock = new(MockBehavior.Strict);
        _fileServiceMock = new(MockBehavior.Strict);
        _loggerMock = new(MockBehavior.Loose);

        _sut = new(
            _releasesClientMock.Object,
            _nextVersionCalculatorMock.Object,
            _environmentServiceMock.Object,
            _fileServiceMock.Object,
            _loggerMock.Object);
    }

    [Test]
    public async Task CreateReleaseAsync_WhenReleaseCanBeCreated_ShouldCallDependenciesAsExpected()
    {
        // arrange
        var input = new ReleaseCreatorOptions("commit sha", ReleaseType.Major, null, true, "access token");
        long repositoryId = 123;
        var outputFilePath = "path/to/file";
        var nextVersion = new SemanticVersion(1, 1, 1, [], 1, []);
        const string ExpectedNextVersion = "1.1.1-1";
        var createdRelease = new Release();

        _environmentServiceMock.Setup(x => x.GetRequiredEnvironmentVariable(KnownConstants.GitHub.EnvironmentVariables.RepositoryId))
            .Returns(repositoryId.ToString());
        _environmentServiceMock.Setup(x => x.GetRequiredEnvironmentVariable(KnownConstants.GitHub.EnvironmentVariables.OutputFilePath))
            .Returns(outputFilePath);

        _fileServiceMock.Setup(x => x.AppendLine(It.IsAny<string>(), It.IsAny<string>()));

        NewRelease? usedNewRelease = null;
        _releasesClientMock.Setup(x => x.Create(It.IsAny<long>(), It.IsAny<NewRelease>()))
            .Callback<long, NewRelease>((_, newRelease) => usedNewRelease = newRelease)
            .ReturnsAsync(createdRelease);

        _nextVersionCalculatorMock.Setup(x => x.CalculateNextVersion(It.IsAny<ReleaseCreatorOptions>()))
            .Returns(nextVersion);

        // act
        var result = await _sut.CreateReleaseAsync(input);

        // assert
        result.Should().Be(createdRelease);

        _environmentServiceMock.Verify(x => x.GetRequiredEnvironmentVariable(KnownConstants.GitHub.EnvironmentVariables.OutputFilePath), Times.Once);
        _environmentServiceMock.Verify(x => x.GetRequiredEnvironmentVariable(KnownConstants.GitHub.EnvironmentVariables.RepositoryId), Times.Once);
        _environmentServiceMock.VerifyNoOtherCalls();

        _fileServiceMock.Verify(x => x.AppendLine(outputFilePath, $"{KnownConstants.GitHub.Action.NextVersionOutputVariableName}={ExpectedNextVersion}"), Times.Once);
        _fileServiceMock.VerifyNoOtherCalls();

        _releasesClientMock.Verify(x => x.Create(repositoryId, It.IsAny<NewRelease>()), Times.Once);
        _releasesClientMock.VerifyNoOtherCalls();
        usedNewRelease!.TagName.Should().Be(ExpectedNextVersion);
        usedNewRelease.Prerelease.Should().BeTrue();
        usedNewRelease.TargetCommitish.Should().Be(input.CommitSha);

        _nextVersionCalculatorMock.Verify(x => x.CalculateNextVersion(input), Times.Once);
        _nextVersionCalculatorMock.VerifyNoOtherCalls();
    }

    [Test]
    public async Task CreateReleaseAsync_WhenRepositoryIdCouldNotBeParsed_ShouldThrowWithMessage()
    {
        // arrange
        const string EnvironmentVariableValue = "not a long";
        var input = new ReleaseCreatorOptions("branch name", ReleaseType.Major, null, true, "access token");
        var nextVersion = new SemanticVersion(1, 1, 1, [], 1, []);

        _environmentServiceMock.Setup(x => x.GetRequiredEnvironmentVariable(It.IsAny<string>()))
            .Returns(EnvironmentVariableValue);

        _nextVersionCalculatorMock.Setup(x => x.CalculateNextVersion(It.IsAny<ReleaseCreatorOptions>()))
            .Returns(nextVersion);

        // act
        var invocation = _sut.Invoking(async x => await x.CreateReleaseAsync(input));

        // assert
        await invocation.Should().ThrowAsync<Exception>()
            .WithMessage($"Expected environment variable '{KnownConstants.GitHub.EnvironmentVariables.RepositoryId}' to be of type '{typeof(long)}' but '{EnvironmentVariableValue}' cannot be parsed.");

        _releasesClientMock.VerifyNoOtherCalls();
    }
}