using FluentAssertions;
using ReleaseCreator.CommandLine.Types;

namespace ReleaseCreator.CommandLine.Tests.Types;

[TestFixture]
public class ReleaseCreatorOptionsTest
{
    [Test]
    public void ToString_ShouldPrintAllPropertiesButAccessToken()
    {
        // arrange
        var testee = new ReleaseCreatorCommandLineOptions("sha", Enums.SemanticReleaseType.Major, "identifier", "access token");

        // act
        var result = testee.ToString();

        // assert
        result.Should().Contain(testee.Commitish);
        result.Should().Contain(testee.SemanticReleaseType.ToString());
        result.Should().Contain(testee.PreReleaseIdentifier);
        result.Should().Contain(testee.IsPreRelease.ToString());
        result.Should().NotContain(testee.AccessToken);
    }
}