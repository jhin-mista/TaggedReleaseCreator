using ReleaseCreator.SemanticVersionUtil.Enums;
using ReleaseCreator.SemanticVersionUtil.Types;

namespace ReleaseCreator.SemanticVersionUtil.Tests.Types;

[TestFixture]
public class SemanticVersionIncrementDtoTest
{
    [TestCase(null, false)]
    [TestCase("identifier", true)]
    [TestCase("", true)]
    public void IsPreRelease_ShouldBeComputedByPreReleaseIdentifier(string? identifier, bool expectedResult)
    {
        // arrange
        var testee = new SemanticVersionIncrementDto(SemanticVersionCorePart.Major, identifier);

        // act
        var isPreRelease = testee.IsPreRelease;

        // assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(isPreRelease, Is.EqualTo(expectedResult));
            Assert.That(testee.IsStableVersion, Is.Not.EqualTo(expectedResult));
        }
    }
}