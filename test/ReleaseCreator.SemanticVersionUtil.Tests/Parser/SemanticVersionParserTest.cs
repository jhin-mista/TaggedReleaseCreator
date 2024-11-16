using FluentAssertions;
using Moq;
using ReleaseCreator.SemanticVersionUtil.Builder;
using ReleaseCreator.SemanticVersionUtil.Parser;
using ReleaseCreator.SemanticVersionUtil.Types;

namespace ReleaseCreator.SemanticVersionUtil.Tests.Parser;

[TestFixture]
public class SemanticVersionParserTest
{
    private SemanticVersionParser _sut;
    private Mock<ISemanticVersionBuilder> _builderMock;

    [SetUp]
    public void SetUp()
    {
        _builderMock = new(MockBehavior.Strict);
        _sut = new(_builderMock.Object);
    }

    [TestCase("v", 1u, 0u, 0u, new string[] { "" }, new string[] { "" }, "v1.0.0", TestName = "Only core version")]
    [TestCase("", 1u, 0u, 0u, new string[] { "alpha" }, new string[] { "" }, "1.0.0-alpha", TestName = "Core + pre-release identifier")]
    [TestCase("", 1u, 0u, 0u, new string[] { "1" }, new string[] { "" }, "1.0.0-1", TestName = "Core + pre-release number")]
    [TestCase("", 1u, 0u, 0u, new string[] { "alpha", "1" }, new string[] { "" }, "1.0.0-alpha.1", TestName = "Core + pre-release version")]
    [TestCase("", 1u, 0u, 0u, new string[] { "" }, new string[] { "156f46a8", "42" }, "1.0.0+156f46a8.42", TestName = "Core version + build metadata")]
    [TestCase("", 1u, 0u, 0u, new string[] { "alpha", "1" }, new string[] { "156f46a8", "42" }, "1.0.0-alpha.1+156f46a8.42", TestName = "Core + pre-release version + build metadata")]
    public void Parse_ShouldParseAsExpected(
        string prefix,
        uint major,
        uint minor,
        uint patch,
        string[] preReleaseVersion,
        string[] buildMetadata,
        string input)
    {
        // arrange
        _builderMock.Setup(x => x.SetMajorVersion(It.IsAny<uint>()));
        _builderMock.Setup(x => x.SetMinorVersion(It.IsAny<uint>()));
        _builderMock.Setup(x => x.SetPatchVersion(It.IsAny<uint>()));

        _builderMock.Setup(x => x.SetPreReleaseVersion(It.IsAny<string?>()));

        _builderMock.Setup(x => x.SetBuildMetadata(It.IsAny<string?>()));

        _builderMock.Setup(x => x.SetPrefix(It.IsAny<string?>()));

        var builderResult = new SemanticVersion(1, 1, 1, [], 1, []);
        _builderMock.Setup(x => x.BuildSemanticVersion()).Returns(builderResult);

        // act
        var result = _sut.Parse(input);

        // assert
        result.Should().Be(builderResult);

        _builderMock.Verify(x => x.SetPrefix(prefix), Times.Once);
        _builderMock.Verify(x => x.SetMajorVersion(major), Times.Once);
        _builderMock.Verify(x => x.SetMinorVersion(minor), Times.Once);
        _builderMock.Verify(x => x.SetPatchVersion(patch), Times.Once);

        _builderMock.Verify(x => x.SetPreReleaseVersion(string.Join('.', preReleaseVersion)), Times.Once);

        _builderMock.Verify(x => x.SetBuildMetadata(string.Join('.', buildMetadata)), Times.Once);

        _builderMock.Verify(x => x.BuildSemanticVersion(), Times.Once);

        _builderMock.VerifyNoOtherCalls();
    }

    [TestCase("invalid")]
    [TestCase("1")]
    [TestCase("1.")]
    [TestCase("1.1")]
    [TestCase("1..1")]
    [TestCase("1.1..1")]
    [TestCase("1.1.1-")]
    [TestCase("1.1.1-+")]
    [TestCase("01.1.1")]
    [TestCase("1.01.1")]
    [TestCase("1.1.01")]
    public void Parse_WhenPassedInvalidInput_ShouldThrowException(string input)
    {
        // act
        var invocation = () => _sut.Parse(input);

        // assert
        invocation.Should().Throw<FormatException>().WithMessage($"'{input}' is not a valid semantic version.");
    }
}