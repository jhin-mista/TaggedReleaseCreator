using FluentAssertions;
using Moq;
using Octokit;

namespace ReleaseCreator.CommandLine.Tests
{
    [TestFixture]
    public class ProgramIntegrationTest
    {
        [SetUp]
        public void SetUp()
        {
            var workingDirectory = Path.Combine(Environment.CurrentDirectory, "..", "..", "..", "TestRepository");
            Environment.CurrentDirectory = workingDirectory;
            Directory.Move(".igit", ".git");
        }

        [TearDown]
        public void TearDown()
        {
            Directory.Move(".git", ".igit");
        }

        [Test]
        public async Task Main_ShouldCreateNextReleaseAndCallClient()
        {
            // arrange
            long repositoryId = -123456789;
            Environment.SetEnvironmentVariable("GITHUB_REPOSITORY_ID", repositoryId.ToString());

            var clientMock = new Mock<IReleasesClient>(MockBehavior.Strict);
            var createdRelease = new Release();
            NewRelease? calculatedNewRelease = null;
            clientMock.Setup(x => x.Create(It.IsAny<long>(), It.IsAny<NewRelease>()))
                .Callback<long, NewRelease>((_, x) => calculatedNewRelease = x)
                .ReturnsAsync(createdRelease);

            Program.ReleasesClientFactory = _ => clientMock.Object;

            string[] arguments =
                [
                    "--branch", "main",
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
            calculatedNewRelease!.TagName.Should().Be("v2.0.0");
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
}