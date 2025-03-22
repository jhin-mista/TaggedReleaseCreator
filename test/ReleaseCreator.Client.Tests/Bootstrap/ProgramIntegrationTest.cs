using FluentAssertions;
using Moq;
using Octokit;
using ReleaseCreator.Client.Bootstrap;
using ReleaseCreator.Client.Extensions;

namespace ReleaseCreator.Client.Tests.Bootstrap;

[TestFixture]
public class ProgramIntegrationTest
{
    private Mock<IReleasesClient> _releasesClientMock;
    private Func<string, IReleasesClient> _releasesClientFactory;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _releasesClientFactory = ServiceCollectionExtension.ReleasesClientFactory;
        var workingDirectory = Path.Combine(Environment.CurrentDirectory, "..", "..", "..", "TestRepository");
        Environment.CurrentDirectory = workingDirectory;
        Directory.Move(".igit", ".git");
    }

    [SetUp]
    public void SetUp()
    {
        _releasesClientMock = new(MockBehavior.Strict);
        ServiceCollectionExtension.ReleasesClientFactory = _ => _releasesClientMock.Object;
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        ServiceCollectionExtension.ReleasesClientFactory = _releasesClientFactory;
        Directory.Move(".git", ".igit");
    }

    [Test]
    public async Task Main_ShouldCreateNextReleaseAndCallClient()
    {
        // arrange
        const string ExpectedTagName = "v2.0.0";
        long repositoryId = -123456789;
        var tmpFilePath = Path.GetTempFileName();
        Environment.SetEnvironmentVariable(KnownConstants.GitHub.EnvironmentVariables.RepositoryId, repositoryId.ToString());
        Environment.SetEnvironmentVariable(KnownConstants.GitHub.EnvironmentVariables.OutputFilePath, tmpFilePath);

        var createdRelease = new Release();
        NewRelease? calculatedNewRelease = null;
        _releasesClientMock.Setup(x => x.Create(It.IsAny<long>(), It.IsAny<NewRelease>()))
            .Callback<long, NewRelease>((_, x) => calculatedNewRelease = x)
            .ReturnsAsync(createdRelease);

        string[] arguments =
            [
                "--commit", "abc123",
                "--type", "major",
                "--token", "token",
            ];

        // act
        await Program.Main(arguments);

        // assert
        Environment.ExitCode.Should().Be(0);

        _releasesClientMock.Verify(x => x.Create(repositoryId, It.IsAny<NewRelease>()), Times.Once);
        _releasesClientMock.VerifyNoOtherCalls();

        calculatedNewRelease.Should().NotBeNull();
        calculatedNewRelease!.TagName.Should().Be(ExpectedTagName);

        var outputFileContents = File.ReadAllLines(tmpFilePath);
        outputFileContents.Should().ContainSingle()
            .Which.Should().Be($"{KnownConstants.GitHub.Action.NextVersionOutputVariableName}={ExpectedTagName}");
    }

    [Test]
    public async Task Main_WhenArgumentParsingFails_ShouldNotCreateRelease()
    {
        // act
        await Program.Main([]);

        // assert
        Environment.ExitCode.Should().NotBe(0);

        _releasesClientMock.Verify(x => x.Create(It.IsAny<long>(), It.IsAny<NewRelease>()), Times.Never);
        _releasesClientMock.VerifyNoOtherCalls();
    }
}