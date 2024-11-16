using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using ReleaseCreator.Git.ShellExecution;

namespace ReleaseCreator.Git.Tests.ShellExecution;

[TestFixture]
class PowerShellExecutorTest
{
    private PowerShellExecutor _sut;
    private Mock<ILogger<PowerShellExecutor>> _loggerMock;

    [SetUp]
    public void SetUp()
    {
        _loggerMock = new(MockBehavior.Loose);
        _sut = new(_loggerMock.Object);
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