using FluentAssertions;
using ReleaseCreator.SemanticVersionUtil.Types;

namespace ReleaseCreator.SemanticVersionUtil.Tests.Types;

[TestFixture]
public class SemanticVersionTest
{
    [TestCase("v", 1u, 0u, 0u, new string[] { }, null, new string[] { }, "1.0.0")]
    [TestCase(null, 1u, 0u, 0u, new string[] { "alpha" }, null, new string[] { }, "1.0.0-alpha")]
    [TestCase(null, 1u, 0u, 0u, new string[] { }, 1u, new string[] { }, "1.0.0-1")]
    [TestCase(null, 1u, 0u, 0u, new string[] { "alpha" }, 1u, new string[] { }, "1.0.0-alpha.1")]
    [TestCase(null, 1u, 0u, 0u, new string[] { }, null, new string[] { "156f46a8", "42" }, "1.0.0+156f46a8.42")]
    [TestCase(null, 1u, 0u, 0u, new string[] { "alpha" }, 1u, new string[] { "156f46a8", "42" }, "1.0.0-alpha.1+156f46a8.42")]
    public void ToString_WhenCalled_ShouldReturnExpectedResult(
        string? prefix,
        uint major,
        uint minor,
        uint patch,
        string[] preReleaseIdentifier,
        uint? preReleaseNumber,
        string[] buildMetadata,
        string expectedResult)
    {
        // arrange
        var testee = new SemanticVersion(major, minor, patch, preReleaseIdentifier, preReleaseNumber, buildMetadata, prefix);

        // act
        var actualResult = testee.ToString();

        // assert
        actualResult.Should().Be(expectedResult);
    }

    [TestCase(null, 1u, 0u, 0u, new string[] { }, null, new string[] { }, "1.0.0")]
    [TestCase("v", 1u, 0u, 0u, new string[] { "alpha" }, null, new string[] { }, "v1.0.0-alpha")]
    [TestCase("v", 1u, 0u, 0u, new string[] { }, 1u, new string[] { }, "v1.0.0-1")]
    [TestCase("v", 1u, 0u, 0u, new string[] { "alpha" }, 1u, new string[] { }, "v1.0.0-alpha.1")]
    [TestCase("v", 1u, 0u, 0u, new string[] { }, null, new string[] { "156f46a8", "42" }, "v1.0.0+156f46a8.42")]
    [TestCase("v", 1u, 0u, 0u, new string[] { "alpha" }, 1u, new string[] { "156f46a8", "42" }, "v1.0.0-alpha.1+156f46a8.42")]
    public void ToStringWithPrefix_WhenGivenPrefix_ShouldReturnExpectedResult(
        string? prefix,
        uint major,
        uint minor,
        uint patch,
        string[] preReleaseIdentifier,
        uint? preReleaseNumber,
        string[] buildMetadata,
        string expectedResult)
    {
        // arrange
        var testee = new SemanticVersion(major, minor, patch, preReleaseIdentifier, preReleaseNumber, buildMetadata, prefix);

        // act
        var actualResult = testee.ToStringWithPrefix();

        // assert
        actualResult.Should().Be(expectedResult);
    }
}