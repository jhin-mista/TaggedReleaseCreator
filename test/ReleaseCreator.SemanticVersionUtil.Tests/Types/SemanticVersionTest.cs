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

    public static IEnumerable<TestCaseData> PreReleaseSemanticVersions
    {
        get
        {
            yield return new(new string[] { "alpha" }, 1u);
            yield return new(new string[] { "alpha" }, null);
            yield return new(Array.Empty<string>(), 1u);
            yield return new(new string[] { string.Empty }, 1u);
            yield return new(new string[] { string.Empty }, null);
        }
    }

    [TestCaseSource(nameof(PreReleaseSemanticVersions))]
    public void IsPreRelease_WhenVersionIsPreRelease_ShouldBeTrue(string[] preReleaseIdentifier, uint? preReleaseNumber)
    {
        // arrange
        var semanticVersion = new SemanticVersion(1, 1, 1, preReleaseIdentifier, preReleaseNumber, []);

        // act
        var isPreRelease = semanticVersion.IsPreRelease;

        // assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(isPreRelease, Is.True);
            Assert.That(semanticVersion.IsStableVersion, Is.False);
        }
    }

    [Test]
    public void IsPreRelease_WhenVersionIsStable_ShouldBeFalse()
    {
        // arrange
        var semanticVersion = new SemanticVersion(1, 1, 1, Array.Empty<string>(), null, []);

        // assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(semanticVersion.IsPreRelease, Is.False);
            Assert.That(semanticVersion.IsStableVersion, Is.True);
        }
    }
}