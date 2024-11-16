using Moq;
using ReleaseCreator.SemanticVersionUtil.Builder;
using ReleaseCreator.SemanticVersionUtil.Enums;
using ReleaseCreator.SemanticVersionUtil.Incrementor;
using ReleaseCreator.SemanticVersionUtil.Types;

namespace ReleaseCreator.SemanticVersionUtil.Tests.Incrementor;

[TestFixture]
public class SemanticVersionIncrementDirectorTest
{
    private SemanticVersionIncrementDirector _sut;
    private Mock<ISemanticVersionBuilder> _builderMock;

    [SetUp]
    public void SetUp()
    {
        _builderMock = new(MockBehavior.Strict);
        _sut = new(_builderMock.Object);
    }

    public static IEnumerable<TestCaseData> PreReleaseToPrereleaseIncreasesWithStaticIdentifier
    {
        get
        {
            // no new pre-release identifier | pre-release number exists
            yield return new("v", new string[] { "alpha" }, 1, SemanticVersionCorePart.Major, string.Empty, 2);
            yield return new("v", new string[] { "alpha" }, 1, SemanticVersionCorePart.Minor, string.Empty, 2);
            yield return new("v", new string[] { "alpha" }, 1, SemanticVersionCorePart.Patch, string.Empty, 2);

            // same pre-release identifier | pre-release number exists
            yield return new("v", new string[] { "alpha" }, 1, SemanticVersionCorePart.Major, "alpha", 2);
            yield return new("v", new string[] { "alpha" }, 1, SemanticVersionCorePart.Minor, "alpha", 2);
            yield return new("v", new string[] { "alpha" }, 1, SemanticVersionCorePart.Patch, "alpha", 2);

            // no new pre-release identifier | no pre-release number exists
            yield return new("v", new string[] { "alpha" }, null, SemanticVersionCorePart.Major, string.Empty, 1);
            yield return new("v", new string[] { "alpha" }, null, SemanticVersionCorePart.Minor, string.Empty, 1);
            yield return new("v", new string[] { "alpha" }, null, SemanticVersionCorePart.Patch, string.Empty, 1);

            // same pre-release identifier | no pre-release number exists
            yield return new("v", new string[] { "alpha" }, null, SemanticVersionCorePart.Major, "alpha", 1);
            yield return new("v", new string[] { "alpha" }, null, SemanticVersionCorePart.Minor, "alpha", 1);
            yield return new("v", new string[] { "alpha" }, null, SemanticVersionCorePart.Patch, "alpha", 1);

            // no current pre-release identifier | no new pre-release identifier
            yield return new("v", Array.Empty<string>(), 1, SemanticVersionCorePart.Major, string.Empty, 2);
            yield return new("v", Array.Empty<string>(), 1, SemanticVersionCorePart.Minor, string.Empty, 2);
            yield return new("v", Array.Empty<string>(), 1, SemanticVersionCorePart.Patch, string.Empty, 2);
            yield return new("v", new string[] { string.Empty }, 1, SemanticVersionCorePart.Major, string.Empty, 2);
            yield return new("v", new string[] { string.Empty }, 1, SemanticVersionCorePart.Minor, string.Empty, 2);
            yield return new("v", new string[] { string.Empty }, 1, SemanticVersionCorePart.Patch, string.Empty, 2);
        }
    }

    [TestCaseSource(nameof(PreReleaseToPrereleaseIncreasesWithStaticIdentifier))]

    public void IncrementPreReleaseToPreRelease_WhenPreReleaseIdentifierDoesNotChange_ShouldCallBuilderAsExpected(
        string? prefix,
        string[] currentPreReleaseIdentifier,
        int? preReleaseNumber,
        SemanticVersionCorePart semanticVersionCorePart,
        string? newPreReleaseIdentifier,
        int? expectedPreReleaseNumber)
    {
        // arrange
        var currentVersion = new SemanticVersion(1, 2, 3, currentPreReleaseIdentifier, (uint?)preReleaseNumber, [], prefix);
        var incrementDto = new SemanticVersionIncrementDto(semanticVersionCorePart, newPreReleaseIdentifier);

        _builderMock.Setup(x => x.SetPrefix(It.IsAny<string?>()));

        _builderMock.Setup(x => x.SetMajorVersion(It.IsAny<uint>()));
        _builderMock.Setup(x => x.SetMinorVersion(It.IsAny<uint>()));
        _builderMock.Setup(x => x.SetPatchVersion(It.IsAny<uint>()));

        _builderMock.Setup(x => x.SetPreReleaseIdentifier(It.IsAny<IList<string>>()));
        _builderMock.Setup(x => x.SetPreReleaseNumber(It.IsAny<uint?>()));

        _builderMock.Setup(x => x.BuildSemanticVersion()).Returns(new SemanticVersion(0, 0, 0, [], null, []));
        _builderMock.Setup(x => x.SetBuildMetadata(It.IsAny<IList<string>>()));

        // act
        _sut.IncrementPreReleaseToPreRelease(currentVersion, incrementDto);

        // assert
        _builderMock.Verify(x => x.SetPrefix(currentVersion.Prefix), Times.Once);

        _builderMock.Verify(x => x.SetMajorVersion(currentVersion.Major), Times.Once);
        _builderMock.Verify(x => x.SetMinorVersion(currentVersion.Minor), Times.Once);
        _builderMock.Verify(x => x.SetPatchVersion(currentVersion.Patch), Times.Once);

        _builderMock.Verify(x => x.SetPreReleaseIdentifier(currentPreReleaseIdentifier), Times.Once);
        _builderMock.Verify(x => x.SetPreReleaseNumber((uint?)expectedPreReleaseNumber), Times.Once);

        _builderMock.Verify(x => x.BuildSemanticVersion(), Times.Once);
        _builderMock.Verify(x => x.SetBuildMetadata(currentVersion.BuildMetadata), Times.Once);

        _builderMock.VerifyNoOtherCalls();
    }

    public static IEnumerable<TestCaseData> PreReleaseToPreReleaseIncreasesChangingIdentifier
    {
        get
        {

            // different pre-release identifier | pre-release number exists
            yield return new("v", new string[] { "alpha" }, 2, SemanticVersionCorePart.Major, "beta", 1);
            yield return new("v", new string[] { "alpha" }, 2, SemanticVersionCorePart.Minor, "beta", 1);
            yield return new("v", new string[] { "alpha" }, 2, SemanticVersionCorePart.Patch, "beta", 1);

            // different pre-release identifier | no pre-release number exists
            yield return new("v", new string[] { "alpha" }, null, SemanticVersionCorePart.Major, "beta", 1);
            yield return new("v", new string[] { "alpha" }, null, SemanticVersionCorePart.Minor, "beta", 1);
            yield return new("v", new string[] { "alpha" }, null, SemanticVersionCorePart.Patch, "beta", 1);

            // no current pre-release identifier | new pre-release identifier
            yield return new("v", Array.Empty<string>(), 2, SemanticVersionCorePart.Major, "alpha", 1);
            yield return new("v", Array.Empty<string>(), 2, SemanticVersionCorePart.Minor, "alpha", 1);
            yield return new("v", Array.Empty<string>(), 2, SemanticVersionCorePart.Patch, "alpha", 1);
        }
    }

    [TestCaseSource(nameof(PreReleaseToPreReleaseIncreasesChangingIdentifier))]
    public void IncrementPreReleaseToPreRelease_WhenPreReleaseIdentifierChanges_ShouldCallBuilderAsExpected(
        string? prefix,
        string[] currentPreReleaseIdentifier,
        int? preReleaseNumber,
        SemanticVersionCorePart semanticVersionCorePart,
        string? newPreReleaseIdentifier,
        int? expectedPreReleaseNumber)
    {
        // arrange
        var currentVersion = new SemanticVersion(1, 2, 3, currentPreReleaseIdentifier, (uint?)preReleaseNumber, [], prefix);
        var incrementDto = new SemanticVersionIncrementDto(semanticVersionCorePart, newPreReleaseIdentifier);

        _builderMock.Setup(x => x.SetPrefix(It.IsAny<string?>()));

        _builderMock.Setup(x => x.SetMajorVersion(It.IsAny<uint>()));
        _builderMock.Setup(x => x.SetMinorVersion(It.IsAny<uint>()));
        _builderMock.Setup(x => x.SetPatchVersion(It.IsAny<uint>()));

        _builderMock.Setup(x => x.SetPreReleaseIdentifier(It.IsAny<string>()));
        _builderMock.Setup(x => x.SetPreReleaseNumber(It.IsAny<uint?>()));

        _builderMock.Setup(x => x.BuildSemanticVersion()).Returns(new SemanticVersion(0, 0, 0, [], null, []));
        _builderMock.Setup(x => x.SetBuildMetadata(It.IsAny<IList<string>>()));

        // act
        _sut.IncrementPreReleaseToPreRelease(currentVersion, incrementDto);

        // assert
        _builderMock.Verify(x => x.SetPrefix(currentVersion.Prefix), Times.Once);

        _builderMock.Verify(x => x.SetMajorVersion(currentVersion.Major), Times.Once);
        _builderMock.Verify(x => x.SetMinorVersion(currentVersion.Minor), Times.Once);
        _builderMock.Verify(x => x.SetPatchVersion(currentVersion.Patch), Times.Once);

        _builderMock.Verify(x => x.SetPreReleaseIdentifier(newPreReleaseIdentifier), Times.Once);
        _builderMock.Verify(x => x.SetPreReleaseNumber((uint?)expectedPreReleaseNumber), Times.Once);

        _builderMock.Verify(x => x.BuildSemanticVersion(), Times.Once);
        _builderMock.Verify(x => x.SetBuildMetadata(currentVersion.BuildMetadata), Times.Once);

        _builderMock.VerifyNoOtherCalls();
    }

    public static IEnumerable<TestCaseData> PreReleaseToStableIncreases
    {
        get
        {
            yield return new("v", new string[] { "alpha" }, 1, SemanticVersionCorePart.Major);
            yield return new("v", new string[] { "alpha" }, 1, SemanticVersionCorePart.Minor);
            yield return new("v", new string[] { "alpha" }, 1, SemanticVersionCorePart.Patch);
        }
    }

    [TestCaseSource(nameof(PreReleaseToStableIncreases))]
    public void IncrementPreReleaseToStable_ShouldCallBuilderAsExpected(
        string? prefix,
        string[] currentPreReleaseIdentifier,
        int? preReleaseNumber,
        SemanticVersionCorePart semanticVersionCorePart)
    {
        // arrange
        var currentVersion = new SemanticVersion(1, 2, 3, currentPreReleaseIdentifier, (uint?)preReleaseNumber, [], prefix);
        var incrementDto = new SemanticVersionIncrementDto(semanticVersionCorePart, null);

        _builderMock.Setup(x => x.SetPrefix(It.IsAny<string?>()));

        _builderMock.Setup(x => x.SetMajorVersion(It.IsAny<uint>()));
        _builderMock.Setup(x => x.SetMinorVersion(It.IsAny<uint>()));
        _builderMock.Setup(x => x.SetPatchVersion(It.IsAny<uint>()));

        _builderMock.Setup(x => x.BuildSemanticVersion()).Returns(new SemanticVersion(0, 0, 0, [], null, []));
        _builderMock.Setup(x => x.SetBuildMetadata(It.IsAny<IList<string>>()));

        // act
        _sut.IncrementPreReleaseToStable(currentVersion, incrementDto);

        // assert
        _builderMock.Verify(x => x.SetPrefix(currentVersion.Prefix), Times.Once);

        _builderMock.Verify(x => x.SetMajorVersion(currentVersion.Major), Times.Once);
        _builderMock.Verify(x => x.SetMinorVersion(currentVersion.Minor), Times.Once);
        _builderMock.Verify(x => x.SetPatchVersion(currentVersion.Patch), Times.Once);

        _builderMock.Verify(x => x.BuildSemanticVersion(), Times.Once);
        _builderMock.Verify(x => x.SetBuildMetadata(currentVersion.BuildMetadata), Times.Once);

        _builderMock.VerifyNoOtherCalls();
    }
    public static IEnumerable<TestCaseData> StableIncreasesToPreRelease
    {
        get
        {
            yield return new("v", 1, 1, 1, "alpha", SemanticVersionCorePart.Major, 2, 0, 0, "alpha");
            yield return new("v", 1, 1, 1, "alpha", SemanticVersionCorePart.Minor, 1, 2, 0, "alpha");
            yield return new("v", 1, 1, 1, "alpha", SemanticVersionCorePart.Patch, 1, 1, 2, "alpha");

            yield return new("v", 1, 1, 1, string.Empty, SemanticVersionCorePart.Major, 2, 0, 0, string.Empty);
            yield return new("v", 1, 1, 1, string.Empty, SemanticVersionCorePart.Minor, 1, 2, 0, string.Empty);
            yield return new("v", 1, 1, 1, string.Empty, SemanticVersionCorePart.Patch, 1, 1, 2, string.Empty);
        }
    }

    [TestCaseSource(nameof(StableIncreasesToPreRelease))]
    public void IncrementStableToPreRelease_ShouldCallBuilderAsExpected(
        string? prefix,
        int major,
        int minor,
        int patch,
        string? nextPreReleaseIdentifier,
        SemanticVersionCorePart semanticVersionCorePart,
        int expectedMajor,
        int expectedMinor,
        int expectedPatch,
        string? expectedPreReleaseIdentifier)
    {
        // arrange
        var currentVersion = new SemanticVersion((uint)major, (uint)minor, (uint)patch, [], null, [], prefix);
        var incrementDto = new SemanticVersionIncrementDto(semanticVersionCorePart, nextPreReleaseIdentifier);

        _builderMock.Setup(x => x.SetPrefix(It.IsAny<string?>()));

        _builderMock.Setup(x => x.SetMajorVersion(It.IsAny<uint>()));
        _builderMock.Setup(x => x.SetMinorVersion(It.IsAny<uint>()));
        _builderMock.Setup(x => x.SetPatchVersion(It.IsAny<uint>()));

        _builderMock.Setup(x => x.SetPreReleaseIdentifier(It.IsAny<string?>()));
        _builderMock.Setup(x => x.SetPreReleaseNumber(It.IsAny<uint?>()));

        _builderMock.Setup(x => x.BuildSemanticVersion()).Returns(new SemanticVersion(0, 0, 0, [], null, []));
        _builderMock.Setup(x => x.SetBuildMetadata(It.IsAny<IList<string>>()));

        // act
        _sut.IncrementStableToPreRelease(currentVersion, incrementDto);

        // assert
        _builderMock.Verify(x => x.SetPrefix(currentVersion.Prefix), Times.Once);

        _builderMock.Verify(x => x.SetMajorVersion((uint)expectedMajor), Times.Once);
        _builderMock.Verify(x => x.SetMinorVersion((uint)expectedMinor), Times.Once);
        _builderMock.Verify(x => x.SetPatchVersion((uint)expectedPatch), Times.Once);

        _builderMock.Verify(x => x.SetPreReleaseIdentifier(expectedPreReleaseIdentifier), Times.Once);
        _builderMock.Verify(x => x.SetPreReleaseNumber((uint?)1), Times.Once);

        _builderMock.Verify(x => x.BuildSemanticVersion(), Times.Once);
        _builderMock.Verify(x => x.SetBuildMetadata(currentVersion.BuildMetadata), Times.Once);

        _builderMock.VerifyNoOtherCalls();
    }

    public static IEnumerable<TestCaseData> StableToStableIncreases
    {
        get
        {
            yield return new("v", 1, 1, 1, SemanticVersionCorePart.Major, 2, 0, 0);
            yield return new("v", 1, 1, 1, SemanticVersionCorePart.Minor, 1, 2, 0);
            yield return new("v", 1, 1, 1, SemanticVersionCorePart.Patch, 1, 1, 2);
        }
    }

    [TestCaseSource(nameof(StableToStableIncreases))]
    public void IncrementStableToStable_ShouldCallBuilderAsExpected(
        string? prefix,
        int major,
        int minor,
        int patch,
        SemanticVersionCorePart semanticVersionCorePart,
        int expectedMajor,
        int expectedMinor,
        int expectedPatch)
    {
        // arrange
        var currentVersion = new SemanticVersion((uint)major, (uint)minor, (uint)patch, [], null, [], prefix);
        var incrementDto = new SemanticVersionIncrementDto(semanticVersionCorePart, null);

        _builderMock.Setup(x => x.SetPrefix(It.IsAny<string?>()));

        _builderMock.Setup(x => x.SetMajorVersion(It.IsAny<uint>()));
        _builderMock.Setup(x => x.SetMinorVersion(It.IsAny<uint>()));
        _builderMock.Setup(x => x.SetPatchVersion(It.IsAny<uint>()));

        _builderMock.Setup(x => x.BuildSemanticVersion()).Returns(new SemanticVersion(0, 0, 0, [], null, []));
        _builderMock.Setup(x => x.SetBuildMetadata(It.IsAny<IList<string>>()));

        // act
        _sut.IncrementStableToStable(currentVersion, incrementDto);

        // assert
        _builderMock.Verify(x => x.SetPrefix(currentVersion.Prefix), Times.Once);

        _builderMock.Verify(x => x.SetMajorVersion((uint)expectedMajor), Times.Once);
        _builderMock.Verify(x => x.SetMinorVersion((uint)expectedMinor), Times.Once);
        _builderMock.Verify(x => x.SetPatchVersion((uint)expectedPatch), Times.Once);

        _builderMock.Verify(x => x.BuildSemanticVersion(), Times.Once);
        _builderMock.Verify(x => x.SetBuildMetadata(currentVersion.BuildMetadata), Times.Once);

        _builderMock.VerifyNoOtherCalls();
    }
}