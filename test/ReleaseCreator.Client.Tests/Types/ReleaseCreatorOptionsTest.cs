using FluentAssertions;
using ReleaseCreator.Client.Types;

namespace ReleaseCreator.Client.Tests.Types;

[TestFixture]
public class ReleaseCreatorOptionsTest
{
    [Test]
    public void ToString_ShouldPrintAllPropertiesButAccessToken()
    {
        // arrange
        var testee = new ReleaseCreatorOptions("sha", Enums.ReleaseType.Major, "identifier", "access token");

        // act
        var result = testee.ToString();

        // assert
        result.Should().Contain(testee.CommitSha);
        result.Should().Contain(testee.VersionIncreasePart.ToString());
        result.Should().Contain(testee.PreReleaseIdentifier);
        result.Should().Contain(testee.IsPreRelease.ToString());
        result.Should().NotContain(testee.AccessToken);
    }
}