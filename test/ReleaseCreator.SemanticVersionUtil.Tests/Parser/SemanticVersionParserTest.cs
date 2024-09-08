using FluentAssertions;
using ReleaseCreator.SemanticVersionUtil.Parser;

namespace ReleaseCreator.SemanticVersionUtil.Tests.Parser;

[TestFixture]
public class SemanticVersionParserTest
{

    [TestCase(1u, 0u, 0u, null, null, null, "1.0.0", TestName = "Only core version")]
    [TestCase(1u, 0u, 0u, new string[] { "alpha" }, null, null, "1.0.0-alpha", TestName = "Core + pre-release identifier")]
    [TestCase(1u, 0u, 0u, null, 1u, null, "1.0.0-1", TestName = "Core + pre-release number")]
    [TestCase(1u, 0u, 0u, new string[] { "alpha" }, 1u, null, "1.0.0-alpha.1", TestName = "Core + pre-release version")]
    [TestCase(1u, 0u, 0u, null, null, new string[] { "156f46a8", "42" }, "1.0.0+156f46a8.42", TestName = "Core version + build metadata")]
    [TestCase(1u, 0u, 0u, new string[] { "alpha" }, 1u, new string[] { "156f46a8", "42" }, "1.0.0-alpha.1+156f46a8.42", TestName = "Core + pre-release version + build metadata")]
    public void Parse_ShouldParseAsExpected(
        uint major,
        uint minor,
        uint patch,
        string[]? preReleaseIdentifier,
        uint? preReleaseNumber,
        string[]? buildMetadata,
        string input)
    {
        // act
        var result = SemanticVersionParser.Parse(input);

        // assert
        result.Major.Should().Be(major);
        result.Minor.Should().Be(minor);
        result.Patch.Should().Be(patch);
        result.PreReleaseIdentifier.Should().BeEquivalentTo(preReleaseIdentifier);
        result.PreReleaseNumber.Should().Be(preReleaseNumber);
        result.BuildMetadata.Should().BeEquivalentTo(buildMetadata);
    }

    [Test]
    public void Parse_WhenPassedInvalidInput_ShouldThrowException()
    {
        // arrange
        var input = "invalid";

        // act
        var invocation = () => SemanticVersionParser.Parse(input);

        // assert
        invocation.Should().Throw<FormatException>().WithMessage($"'{input}' is not a valid semantic version.");
    }
}