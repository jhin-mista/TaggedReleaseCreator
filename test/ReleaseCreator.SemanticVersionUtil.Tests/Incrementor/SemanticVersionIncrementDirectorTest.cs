using Moq;
using ReleaseCreator.SemanticVersionUtil.Builder;
using ReleaseCreator.SemanticVersionUtil.Enums;
using ReleaseCreator.SemanticVersionUtil.Incrementor;
using ReleaseCreator.SemanticVersionUtil.Types;

namespace ReleaseCreator.SemanticVersionUtil.Tests.Incrementor
{
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

        public static IEnumerable<TestCaseData> PreReleaseToPreReleaseIncreases
        {
            get
            {
                // no new pre-release identifier | pre-release number exists
                yield return new("v", new string[] { "alpha" }, 1, SemanticVersionCorePart.Major, null, new string[] { "alpha" }, 2);
                yield return new("v", new string[] { "alpha" }, 1, SemanticVersionCorePart.Minor, null, new string[] { "alpha" }, 2);
                yield return new("v", new string[] { "alpha" }, 1, SemanticVersionCorePart.Patch, null, new string[] { "alpha" }, 2);

                // same pre-release identifier | pre-release number exists
                yield return new("v", new string[] { "alpha" }, 1, SemanticVersionCorePart.Major, "alpha", new string[] { "alpha" }, 2);
                yield return new("v", new string[] { "alpha" }, 1, SemanticVersionCorePart.Minor, "alpha", new string[] { "alpha" }, 2);
                yield return new("v", new string[] { "alpha" }, 1, SemanticVersionCorePart.Patch, "alpha", new string[] { "alpha" }, 2);

                // different pre-release identifier | pre-release number exists
                yield return new("v", new string[] { "alpha" }, 1, SemanticVersionCorePart.Major, "beta", new string[] { "beta" }, 1);
                yield return new("v", new string[] { "alpha" }, 1, SemanticVersionCorePart.Minor, "beta", new string[] { "beta" }, 1);
                yield return new("v", new string[] { "alpha" }, 1, SemanticVersionCorePart.Patch, "beta", new string[] { "beta" }, 1);

                // no new pre-release identifier | no pre-release number exists
                yield return new("v", new string[] { "alpha" }, null, SemanticVersionCorePart.Major, null, new string[] { "alpha" }, 1);
                yield return new("v", new string[] { "alpha" }, null, SemanticVersionCorePart.Minor, null, new string[] { "alpha" }, 1);
                yield return new("v", new string[] { "alpha" }, null, SemanticVersionCorePart.Patch, null, new string[] { "alpha" }, 1);

                // same pre-release identifier | no pre-release number exists
                yield return new("v", new string[] { "alpha" }, null, SemanticVersionCorePart.Major, "alpha", new string[] { "alpha" }, 1);
                yield return new("v", new string[] { "alpha" }, null, SemanticVersionCorePart.Minor, "alpha", new string[] { "alpha" }, 1);
                yield return new("v", new string[] { "alpha" }, null, SemanticVersionCorePart.Patch, "alpha", new string[] { "alpha" }, 1);

                // different pre-release identifier | no pre-release number exists
                yield return new("v", new string[] { "alpha" }, null, SemanticVersionCorePart.Major, "beta", new string[] { "beta" }, 1);
                yield return new("v", new string[] { "alpha" }, null, SemanticVersionCorePart.Minor, "beta", new string[] { "beta" }, 1);
                yield return new("v", new string[] { "alpha" }, null, SemanticVersionCorePart.Patch, "beta", new string[] { "beta" }, 1);

                // no current pre-release identifier | no new pre-release identifier
                yield return new("v", null, 1, SemanticVersionCorePart.Major, null, null, 2);
                yield return new("v", null, 1, SemanticVersionCorePart.Minor, null, null, 2);
                yield return new("v", null, 1, SemanticVersionCorePart.Patch, null, null, 2);

                // no current pre-release identifier | new pre-release identifier
                yield return new("v", null, 2, SemanticVersionCorePart.Major, "alpha", new string[] { "alpha" }, 1);
                yield return new("v", null, 2, SemanticVersionCorePart.Minor, "alpha", new string[] { "alpha" }, 1);
                yield return new("v", null, 2, SemanticVersionCorePart.Patch, "alpha", new string[] { "alpha" }, 1);
            }
        }

        [TestCaseSource(nameof(PreReleaseToPreReleaseIncreases))]
        public void IncrementPreReleaseToPreRelease_ShouldIncrementAsExpected(
            string? prefix,
            string[]? currentPreReleaseIdentifier,
            int? preReleaseNumber,
            SemanticVersionCorePart semanticVersionCorePart,
            string? newPreReleaseIdentifier,
            string[]? expectedPreReleaseIdentifier,
            int? expectedPreReleaseNumber)
        {
            // arrange
            var currentVersion = new SemanticVersion(1, 2, 3, currentPreReleaseIdentifier, (uint?)preReleaseNumber, null, prefix);
            var incrementDto = new SemanticVersionIncrementDto(semanticVersionCorePart, newPreReleaseIdentifier, true);

            _builderMock.Setup(x => x.SetPrefix(It.IsAny<string?>()));

            _builderMock.Setup(x => x.SetMajorVersion(It.IsAny<uint>()));
            _builderMock.Setup(x => x.SetMinorVersion(It.IsAny<uint>()));
            _builderMock.Setup(x => x.SetPatchVersion(It.IsAny<uint>()));

            _builderMock.Setup(x => x.SetPreReleaseIdentifier(It.IsAny<IList<string>?>()));
            _builderMock.Setup(x => x.SetPreReleaseNumber(It.IsAny<uint?>()));

            _builderMock.Setup(x => x.GetSemanticVersion()).Returns(new SemanticVersion(0, 0, 0, null, null, null));
            _builderMock.Setup(x => x.SetBuildMetadata(It.IsAny<IList<string>>()));

            // act
            _sut.IncrementPreReleaseToPreRelease(currentVersion, incrementDto);

            // assert
            _builderMock.Verify(x => x.SetPrefix(currentVersion.Prefix), Times.Once);

            _builderMock.Verify(x => x.SetMajorVersion(currentVersion.Major), Times.Once);
            _builderMock.Verify(x => x.SetMinorVersion(currentVersion.Minor), Times.Once);
            _builderMock.Verify(x => x.SetPatchVersion(currentVersion.Patch), Times.Once);

            _builderMock.Verify(x => x.SetPreReleaseIdentifier(expectedPreReleaseIdentifier), Times.Once);
            _builderMock.Verify(x => x.SetPreReleaseNumber((uint?)expectedPreReleaseNumber), Times.Once);

            _builderMock.Verify(x => x.GetSemanticVersion(), Times.Once);
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
        public void IncrementPreReleaseToStable_ShouldIncrementAsExpected(
            string? prefix,
            string[]? currentPreReleaseIdentifier,
            int? preReleaseNumber,
            SemanticVersionCorePart semanticVersionCorePart)
        {
            // arrange
            var currentVersion = new SemanticVersion(1, 2, 3, currentPreReleaseIdentifier, (uint?)preReleaseNumber, null, prefix);
            var incrementDto = new SemanticVersionIncrementDto(semanticVersionCorePart, null, false);

            _builderMock.Setup(x => x.SetPrefix(It.IsAny<string?>()));

            _builderMock.Setup(x => x.SetMajorVersion(It.IsAny<uint>()));
            _builderMock.Setup(x => x.SetMinorVersion(It.IsAny<uint>()));
            _builderMock.Setup(x => x.SetPatchVersion(It.IsAny<uint>()));

            _builderMock.Setup(x => x.GetSemanticVersion()).Returns(new SemanticVersion(0, 0, 0, null, null, null));
            _builderMock.Setup(x => x.SetBuildMetadata(It.IsAny<IList<string>>()));

            // act
            _sut.IncrementPreReleaseToStable(currentVersion, incrementDto);

            // assert
            _builderMock.Verify(x => x.SetPrefix(currentVersion.Prefix), Times.Once);

            _builderMock.Verify(x => x.SetMajorVersion(currentVersion.Major), Times.Once);
            _builderMock.Verify(x => x.SetMinorVersion(currentVersion.Minor), Times.Once);
            _builderMock.Verify(x => x.SetPatchVersion(currentVersion.Patch), Times.Once);

            _builderMock.Verify(x => x.GetSemanticVersion(), Times.Once);
            _builderMock.Verify(x => x.SetBuildMetadata(currentVersion.BuildMetadata), Times.Once);

            _builderMock.VerifyNoOtherCalls();
        }
        public static IEnumerable<TestCaseData> StableIncreasesToPreRelease
        {
            get
            {
                yield return new("v", 1, 1, 1, "alpha", SemanticVersionCorePart.Major, 2, 0, 0, new string[] { "alpha" });
                yield return new("v", 1, 1, 1, "alpha", SemanticVersionCorePart.Minor, 1, 2, 0, new string[] { "alpha" });
                yield return new("v", 1, 1, 1, "alpha", SemanticVersionCorePart.Patch, 1, 1, 2, new string[] { "alpha" });

                yield return new("v", 1, 1, 1, null, SemanticVersionCorePart.Major, 2, 0, 0, null);
                yield return new("v", 1, 1, 1, null, SemanticVersionCorePart.Minor, 1, 2, 0, null);
                yield return new("v", 1, 1, 1, null, SemanticVersionCorePart.Patch, 1, 1, 2, null);
            }
        }

        [TestCaseSource(nameof(StableIncreasesToPreRelease))]
        public void IncrementStableToPreRelease_ShouldIncrementAsExpected(
            string? prefix,
            int major,
            int minor,
            int patch,
            string? nextPreReleaseIdentifier,
            SemanticVersionCorePart semanticVersionCorePart,
            int expectedMajor,
            int expectedMinor,
            int expectedPatch,
            string[]? expectedPreReleaseIdentifier)
        {
            // arrange
            var currentVersion = new SemanticVersion((uint)major, (uint)minor, (uint)patch, null, null, null, prefix);
            var incrementDto = new SemanticVersionIncrementDto(semanticVersionCorePart, nextPreReleaseIdentifier, true);

            _builderMock.Setup(x => x.SetPrefix(It.IsAny<string?>()));

            _builderMock.Setup(x => x.SetMajorVersion(It.IsAny<uint>()));
            _builderMock.Setup(x => x.SetMinorVersion(It.IsAny<uint>()));
            _builderMock.Setup(x => x.SetPatchVersion(It.IsAny<uint>()));

            _builderMock.Setup(x => x.SetPreReleaseIdentifier(It.IsAny<IList<string>?>()));
            _builderMock.Setup(x => x.SetPreReleaseNumber(It.IsAny<uint?>()));

            _builderMock.Setup(x => x.GetSemanticVersion()).Returns(new SemanticVersion(0, 0, 0, null, null, null));
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

            _builderMock.Verify(x => x.GetSemanticVersion(), Times.Once);
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
        public void IncrementStableToStable_ShouldIncrementAsExpected(
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
            var currentVersion = new SemanticVersion((uint)major, (uint)minor, (uint)patch, null, null, null, prefix);
            var incrementDto = new SemanticVersionIncrementDto(semanticVersionCorePart, null, false);

            _builderMock.Setup(x => x.SetPrefix(It.IsAny<string?>()));

            _builderMock.Setup(x => x.SetMajorVersion(It.IsAny<uint>()));
            _builderMock.Setup(x => x.SetMinorVersion(It.IsAny<uint>()));
            _builderMock.Setup(x => x.SetPatchVersion(It.IsAny<uint>()));

            _builderMock.Setup(x => x.GetSemanticVersion()).Returns(new SemanticVersion(0, 0, 0, null, null, null));
            _builderMock.Setup(x => x.SetBuildMetadata(It.IsAny<IList<string>>()));

            // act
            _sut.IncrementStableToStable(currentVersion, incrementDto);

            // assert
            _builderMock.Verify(x => x.SetPrefix(currentVersion.Prefix), Times.Once);

            _builderMock.Verify(x => x.SetMajorVersion((uint)expectedMajor), Times.Once);
            _builderMock.Verify(x => x.SetMinorVersion((uint)expectedMinor), Times.Once);
            _builderMock.Verify(x => x.SetPatchVersion((uint)expectedPatch), Times.Once);

            _builderMock.Verify(x => x.GetSemanticVersion(), Times.Once);
            _builderMock.Verify(x => x.SetBuildMetadata(currentVersion.BuildMetadata), Times.Once);

            _builderMock.VerifyNoOtherCalls();
        }
    }
}