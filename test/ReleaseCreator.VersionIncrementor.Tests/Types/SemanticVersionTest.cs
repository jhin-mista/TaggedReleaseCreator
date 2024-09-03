using FluentAssertions;
using ReleaseCreator.VersionIncrementor.Types;

namespace ReleaseCreator.VersionIncrementor.Tests.Types;

[TestFixture]
public class SemanticVersionTest
{
    [TestCase(1, 0, 0, null, null, null, "1.0.0", TestName = "Only core version")]
    [TestCase(1, 0, 0, new string[] { "alpha" }, null, null, "1.0.0-alpha", TestName = "Core + pre-release identifier")]
    [TestCase(1, 0, 0, null, 1, null, "1.0.0-1", TestName = "Core + pre-release number")]
    [TestCase(1, 0, 0, new string[] { "alpha" }, 1, null, "1.0.0-alpha.1", TestName = "Core + pre-release version")]
    [TestCase(1, 0, 0, null, null, new string[] { "156f46a8", "42" }, "1.0.0+156f46a8.42", TestName = "Core version + build metadata")]
    [TestCase(1, 0, 0, new string[] { "alpha" }, 1, new string[] { "156f46a8", "42" }, "1.0.0-alpha.1+156f46a8.42", TestName = "Core + pre-release version + build metadata")]
    public void ToString_WhenCalled_ShouldReturnExpectedResult(
        int major,
        int minor,
        int patch,
        string[]? preReleaseIdentifier,
        int? preReleaseNumber,
        string[]? buildMetadata,
        string expectedResult)
    {
        // arrange
        var testee = new SemanticVersion((uint)major, (uint)minor, (uint)patch, preReleaseIdentifier, (uint?)preReleaseNumber, buildMetadata);

        // act
        var actualResult = testee.ToString();

        // assert
        actualResult.Should().Be(expectedResult);
    }
}