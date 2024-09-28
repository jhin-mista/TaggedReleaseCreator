using FluentAssertions;
using ReleaseCreator.SemanticVersionUtil.Builder;

namespace ReleaseCreator.SemanticVersionUtil.Tests.Builder;

[TestFixture]
public class SemanticVersionBuilderTest
{
    private SemanticVersionBuilder _sut;

    [SetUp]
    public void SetUp()
    {
        _sut = new();
    }

    [Test]
    public void GetSemanticVersion_ShouldBuildSemanticVersionAsExpected()
    {
        // act
        var result = _sut.GetSemanticVersion();

        // assert
        result.Major.Should().Be(default);
        result.Minor.Should().Be(default);
        result.Patch.Should().Be(default);

        result.PreReleaseIdentifier.Should().BeNull();
        result.PreReleaseNumber.Should().BeNull();

        result.BuildMetadata.Should().BeNull();
    }

    [Test]
    public void SetMajorVersion_ShouldOnlySetMajorVersion()
    {
        // arrange
        uint major = 42;

        // act
        _sut.SetMajorVersion(major);

        // assert
        var result = _sut.GetSemanticVersion();
        result.Major.Should().Be(major);
        result.Minor.Should().Be(default);
        result.Patch.Should().Be(default);

        result.PreReleaseIdentifier.Should().BeNull();
        result.PreReleaseNumber.Should().BeNull();

        result.BuildMetadata.Should().BeNull();
    }

    [Test]
    public void SetMinorVersion_ShouldOnlySetMinorVersion()
    {
        // arrange
        uint minor = 42;

        // act
        _sut.SetMinorVersion(minor);

        // assert
        var result = _sut.GetSemanticVersion();
        result.Major.Should().Be(default);
        result.Minor.Should().Be(minor);
        result.Patch.Should().Be(default);

        result.PreReleaseIdentifier.Should().BeNull();
        result.PreReleaseNumber.Should().BeNull();

        result.BuildMetadata.Should().BeNull();
    }

    [Test]
    public void SetPatchVersion_ShouldOnlySetPatchVersion()
    {
        // arrange
        uint patch = 42;

        // act
        _sut.SetPatchVersion(patch);

        // assert
        var result = _sut.GetSemanticVersion();
        result.Major.Should().Be(default);
        result.Minor.Should().Be(default);
        result.Patch.Should().Be(patch);

        result.PreReleaseIdentifier.Should().BeNull();
        result.PreReleaseNumber.Should().BeNull();

        result.BuildMetadata.Should().BeNull();
    }

    [Test]
    public void SetPreReleaseIdentifier_ShouldOnlySetPreReleaseIdentifier()
    {
        // arrange
        var preReleaseIdentifier = new[] { "test" };

        // act
        _sut.SetPreReleaseIdentifier(preReleaseIdentifier);

        // assert
        var result = _sut.GetSemanticVersion();
        result.Major.Should().Be(default);
        result.Minor.Should().Be(default);
        result.Patch.Should().Be(default);

        result.PreReleaseIdentifier.Should().BeEquivalentTo(preReleaseIdentifier);
        result.PreReleaseNumber.Should().BeNull();

        result.BuildMetadata.Should().BeNull();
    }

    [Test]
    public void SetPreReleaseNumber_ShouldOnlySetPreReleaseNumber()
    {
        // arrange
        uint? preReleaseNumber = 42;

        // act
        _sut.SetPreReleaseNumber(preReleaseNumber);

        // assert
        var result = _sut.GetSemanticVersion();
        result.Major.Should().Be(default);
        result.Minor.Should().Be(default);
        result.Patch.Should().Be(default);

        result.PreReleaseIdentifier.Should().BeNull();
        result.PreReleaseNumber.Should().Be(preReleaseNumber);

        result.BuildMetadata.Should().BeNull();
    }

    [Test]
    public void SetBuildMetadata_ShouldOnlySetBuildMetadata()
    {
        // arrange
        var buildMetadata = new[] { "test" };

        // act
        _sut.SetBuildMetadata(buildMetadata);

        // assert
        var result = _sut.GetSemanticVersion();
        result.Major.Should().Be(default);
        result.Minor.Should().Be(default);
        result.Patch.Should().Be(default);

        result.PreReleaseIdentifier.Should().BeNull();
        result.PreReleaseNumber.Should().BeNull();

        result.BuildMetadata.Should().BeEquivalentTo(buildMetadata);
    }
}