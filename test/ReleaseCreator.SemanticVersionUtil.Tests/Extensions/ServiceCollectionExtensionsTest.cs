using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using ReleaseCreator.SemanticVersionUtil.Extensions;

namespace ReleaseCreator.SemanticVersionUtil.Tests.Extensions;
[TestFixture]
public class ServiceCollectionExtensionsTest
{
    private ServiceCollection _testee;

    [SetUp]
    public void SetUp()
    {
        _testee = new ServiceCollection();
    }

    [Test]
    public void AddReleaseCreatorClientServicesSingleton_ShouldNotThrow()
    {
        // act
        var invocation = _testee.Invoking(x => x.AddSemanticVersionUtilServicesSingleton());

        // assert
        invocation.Should().NotThrow();
    }
}