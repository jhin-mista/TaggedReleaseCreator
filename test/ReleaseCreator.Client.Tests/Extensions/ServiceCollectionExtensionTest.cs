using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using ReleaseCreator.Client.Extensions;
using ReleaseCreator.Client.ReleaseCreation;

namespace ReleaseCreator.Client.Tests.Extensions;

[TestFixture]
public class ServiceCollectionExtensionTest
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
        var result = _testee.AddReleaseCreatorClientServicesSingleton("token");

        // assert
        var provider = result.BuildServiceProvider();
        provider.Invoking(x => x.GetRequiredService<IReleaseCreator>()).Should().NotThrow();
    }
}