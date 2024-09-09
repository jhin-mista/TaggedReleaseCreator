using FluentAssertions;
using Moq;
using System.Management.Automation;

namespace ReleaseCreator.Git.Tests
{
    [TestFixture]
    public class TagRetrieverTest
    {
        private TagRetriever _sut;
        private Mock<IPowerShellExecutor> _powerShellExecutorMock;

        [SetUp]
        public void SetUp()
        {
            _powerShellExecutorMock = new(MockBehavior.Strict);
            _sut = new(_powerShellExecutorMock.Object);
        }

        [Test]
        public void GetLatestTag_WhenTagExists_ShouldReturnTagName()
        {
            // arrange
            var path = "path/to/somewhere";
            var expectedTagName = "tag";
            var powerShellResult = new PSObject(expectedTagName);

            _powerShellExecutorMock.Setup(x => x.Execute(It.IsAny<string>())).Returns([powerShellResult]);

            // act
            var actualTagName = _sut.GetLatestTag(path);

            // assert
            var expectedScript = $@"Set-Location {path}
git tag --sort=-v:refname | Select-Object -First 1";

            actualTagName.Should().Be(expectedTagName);

            _powerShellExecutorMock.Verify(x => x.Execute(expectedScript), Times.Once);
            _powerShellExecutorMock.VerifyNoOtherCalls();
        }
    }
}
