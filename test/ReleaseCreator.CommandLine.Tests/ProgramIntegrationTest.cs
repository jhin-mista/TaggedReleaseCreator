using FluentAssertions;
using Moq;
using Octokit;

namespace ReleaseCreator.Client.Tests;

[TestFixture]
public class ProgramIntegrationTest
{
    [OneTimeSetUp]
    public void SetUp()
    {
        var workingDirectory = Path.Combine(Environment.CurrentDirectory, "..", "..", "..", "TestRepository");
        Environment.CurrentDirectory = workingDirectory;
        Directory.Move(".igit", ".git");
    }

    [OneTimeTearDown]
    public void TearDown()
    {
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

        var clientMock = new Mock<IReleasesClient>(MockBehavior.Strict);
        var createdRelease = new Release();
        NewRelease? calculatedNewRelease = null;
        clientMock.Setup(x => x.Create(It.IsAny<long>(), It.IsAny<NewRelease>()))
            .Callback<long, NewRelease>((_, x) => calculatedNewRelease = x)
            .ReturnsAsync(createdRelease);

        Program.ReleasesClientFactory = _ => clientMock.Object;

        string[] arguments =
            [
                "--commit", "abc123",
                "--type", "major",
                "--token", "token",
            ];

        // act
        var result = await Program.Main(arguments);

        // assert
        result.Should().Be(0);

        clientMock.Verify(x => x.Create(repositoryId, It.IsAny<NewRelease>()), Times.Once);
        clientMock.VerifyNoOtherCalls();

        calculatedNewRelease.Should().NotBeNull();
        calculatedNewRelease!.TagName.Should().Be(ExpectedTagName);

        var outputFileContents = File.ReadAllLines(tmpFilePath);
        outputFileContents.Should().ContainSingle()
            .Which.Should().Be($"{KnownConstants.GitHub.Action.NextVersionOutputVariableName}={ExpectedTagName}");
    }

    [Test]
    public async Task Main_WhenArgumentParsingFails_ShouldNotCreateRelease()
    {
        // arrange
        var clientMock = new Mock<IReleasesClient>(MockBehavior.Strict);
        Program.ReleasesClientFactory = _ => clientMock.Object;

        // act
        var result = await Program.Main([]);

        // assert
        result.Should().NotBe(0);

        clientMock.Verify(x => x.Create(It.IsAny<long>(), It.IsAny<NewRelease>()), Times.Never);
    }
}