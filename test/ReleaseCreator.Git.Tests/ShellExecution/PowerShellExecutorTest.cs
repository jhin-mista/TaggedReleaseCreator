using FluentAssertions;
using ReleaseCreator.Git.ShellExecution;

namespace ReleaseCreator.Git.Tests.ShellExecution
{
    [TestFixture]
    class PowerShellExecutorTest
    {
        private PowerShellExecutor _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new();
        }

        [Test]
        public void Execute_ShouldExecuteCommandAndReturnExpectedResult()
        {
            // arrange
            var expectedResult = "test";
            var script = $"echo {expectedResult}";

            // act
            var result = _sut.Execute(script);

            // assert
            result.Should().ContainSingle();

            result.Single().BaseObject.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void Execute_WhenScriptErroneous_ShouldThrow()
        {
            // arrange
            var script = "not a known command";

            // act
            var invocation = _sut.Invoking(x => x.Execute(script));

            // assert
            invocation.Should().Throw<AggregateException>().WithMessage($"Error(s) executing script '{script}'*");
        }
    }
}