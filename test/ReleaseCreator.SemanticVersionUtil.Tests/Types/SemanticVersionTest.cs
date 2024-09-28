using FluentAssertions;
using ReleaseCreator.SemanticVersionUtil.Types;

namespace ReleaseCreator.SemanticVersionUtil.Tests.Types;

[TestFixture]
public class SemanticVersionTest
{
    [TestCase(1u, 0u, 0u, null, null, null, "1.0.0", TestName = "Only core version")]
    [TestCase(1u, 0u, 0u, new string[] { "alpha" }, null, null, "1.0.0-alpha", TestName = "Core + pre-release identifier")]
    [TestCase(1u, 0u, 0u, null, 1u, null, "1.0.0-1", TestName = "Core + pre-release number")]
    [TestCase(1u, 0u, 0u, new string[] { "alpha" }, 1u, null, "1.0.0-alpha.1", TestName = "Core + pre-release version")]
    [TestCase(1u, 0u, 0u, null, null, new string[] { "156f46a8", "42" }, "1.0.0+156f46a8.42", TestName = "Core version + build metadata")]
    [TestCase(1u, 0u, 0u, new string[] { "alpha" }, 1u, new string[] { "156f46a8", "42" }, "1.0.0-alpha.1+156f46a8.42", TestName = "Core + pre-release version + build metadata")]
    public void ToString_WhenCalled_ShouldReturnExpectedResult(
        uint major,
        uint minor,
        uint patch,
        string[]? preReleaseIdentifier,
        uint? preReleaseNumber,
        string[]? buildMetadata,
        string expectedResult)
    {
        // arrange
        var testee = new SemanticVersion(major, minor, patch, preReleaseIdentifier, preReleaseNumber, buildMetadata);

        // act
        var actualResult = testee.ToString();

        // assert
        actualResult.Should().Be(expectedResult);
    }
}