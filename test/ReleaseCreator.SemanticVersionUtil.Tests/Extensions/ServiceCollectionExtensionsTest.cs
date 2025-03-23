using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using ReleaseCreator.SemanticVersionUtil.Extensions;
using ReleaseCreator.SemanticVersionUtil.Incrementor;
using ReleaseCreator.SemanticVersionUtil.Parser;

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
        var result = _testee.AddSemanticVersionUtilServicesSingleton();

        // assert
        var provider = result.BuildServiceProvider();
        provider.Invoking(x => x.GetRequiredService<ISemanticVersionParser>()).Should().NotThrow();
        provider.Invoking(x => x.GetRequiredService<ISemanticVersionIncrementor>()).Should().NotThrow();
    }
}