using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Octokit;
using ReleaseCreator.CommandLine.Enums;
using ReleaseCreator.CommandLine.ReleaseCreation.GitHub;
using ReleaseCreator.CommandLine.Types;
using ReleaseCreator.CommandLine.Util;
using ReleaseCreator.CommandLine.VersionCalculation;
using ReleaseCreator.SemanticVersionUtil.Types;

namespace ReleaseCreator.CommandLine.Tests.ReleaseCreation.GitHub
{
    [TestFixture]
    public class GitHubReleaseCreatorTest
    {
        private GitHubReleaseCreator _sut;
        private Mock<IReleasesClient> _releasesClientMock;
        private Mock<INextVersionCalculator> _nextVersionCalculatorMock;
        private Mock<IEnvironmentService> _environmentServiceMock;
        private Mock<ILogger<GitHubReleaseCreator>> _loggerMock;

        [SetUp]
        public void SetUp()
        {
            _releasesClientMock = new(MockBehavior.Strict);
            _nextVersionCalculatorMock = new(MockBehavior.Strict);
            _environmentServiceMock = new(MockBehavior.Strict);
            _loggerMock = new(MockBehavior.Loose);

            _sut = new(
                _releasesClientMock.Object,
                _nextVersionCalculatorMock.Object,
                _environmentServiceMock.Object,
                _loggerMock.Object);
        }

        [Test]
        public async Task CreateReleaseAsync_WhenReleaseCanBeCreated_ShouldCallDependenciesAsExpected()
        {
            // arrange
            var input = new ReleaseCreatorOptions("branch name", ReleaseType.Major, null, true, "access token");
            long releaseId = 123;
            var nextVersion = new SemanticVersion(1, 1, 1, null, 1, null);
            var createdRelease = new Release();

            _environmentServiceMock.Setup(x => x.GetEnvironmentVariable(It.IsAny<string>()))
                .Returns(releaseId.ToString());

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

            _environmentServiceMock.Verify(x => x.GetEnvironmentVariable("GITHUB_REPOSITORY_ID"), Times.Once);
            _environmentServiceMock.VerifyNoOtherCalls();

            _releasesClientMock.Verify(x => x.Create(releaseId, It.IsAny<NewRelease>()), Times.Once);
            _releasesClientMock.VerifyNoOtherCalls();
            usedNewRelease!.TagName.Should().Be("v1.1.1-1");
            usedNewRelease.Prerelease.Should().BeTrue();
            usedNewRelease.TargetCommitish.Should().Be(input.BranchName);

            _nextVersionCalculatorMock.Verify(x => x.CalculateNextVersion(input), Times.Once);
            _nextVersionCalculatorMock.VerifyNoOtherCalls();
        }

        [Test]
        public async Task CreateReleaseAsync_WhenEnvironmentVariableNotSet_ShouldThrowWithMessage()
        {
            // arrange
            var input = new ReleaseCreatorOptions("branch name", ReleaseType.Major, null, true, "access token");
            var nextVersion = new SemanticVersion(1, 1, 1, null, 1, null);

            _environmentServiceMock.Setup(x => x.GetEnvironmentVariable(It.IsAny<string>()))
                .Returns((string?)null);

            _nextVersionCalculatorMock.Setup(x => x.CalculateNextVersion(It.IsAny<ReleaseCreatorOptions>()))
                .Returns(nextVersion);

            // act
            var invocation = _sut.Invoking(async x => await x.CreateReleaseAsync(input));

            // assert
            await invocation.Should().ThrowAsync<Exception>().WithMessage($"Environment variable 'GITHUB_REPOSITORY_ID' is not set but required.");

            _releasesClientMock.VerifyNoOtherCalls();
        }

        [Test]
        public async Task CreateReleaseAsync_WhenRepositoryIdCouldNotBeParsed_ShouldThrowWithMessage()
        {
            // arrange
            const string EnvironmentVariableValue = "not a long";
            var input = new ReleaseCreatorOptions("branch name", ReleaseType.Major, null, true, "access token");
            var nextVersion = new SemanticVersion(1, 1, 1, null, 1, null);

            _environmentServiceMock.Setup(x => x.GetEnvironmentVariable(It.IsAny<string>()))
                .Returns(EnvironmentVariableValue);

            _nextVersionCalculatorMock.Setup(x => x.CalculateNextVersion(It.IsAny<ReleaseCreatorOptions>()))
                .Returns(nextVersion);

            // act
            var invocation = _sut.Invoking(async x => await x.CreateReleaseAsync(input));

            // assert
            await invocation.Should().ThrowAsync<Exception>()
                .WithMessage($"Expected environment variable 'GITHUB_REPOSITORY_ID' to be of type '{typeof(long)}' but '{EnvironmentVariableValue}' cannot be parsed.");

            _releasesClientMock.VerifyNoOtherCalls();
        }
    }
}