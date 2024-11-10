using FluentAssertions;
using ReleaseCreator.Client.Util;

namespace ReleaseCreator.Client.Tests.Util;

[TestFixture]
public class EnvironmentServiceTest
{
    private EnvironmentService _sut;

    [SetUp]
    public void SetUp()
    {
        _sut = new();
    }

    [Test]
    public void GetRequiredEnvironmentVariable_ShouldReturnExpectedValue()
    {
        // arrange
        var key = Guid.NewGuid().ToString();
        var value = "test";

        Environment.SetEnvironmentVariable(key, value);

        // act
        var result = _sut.GetRequiredEnvironmentVariable(key);

        // assert
        result.Should().Be(value);
    }

    [Test]
    public void GetRequiredEnvironmentVariable_WhenVariableDoesNotExist_ShouldThrowExceptionWithMessage()
    {
        // arrange
        var key = Guid.NewGuid().ToString();

        // act
        var invocation = _sut.Invoking(x => x.GetRequiredEnvironmentVariable(key));

        // assert
        invocation.Should().Throw<Exception>().WithMessage($"Environment variable '{key}' is not set but required.");
    }
}