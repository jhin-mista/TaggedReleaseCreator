using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using ReleaseCreator.Client.Extensions;

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
        var invocation = _testee.Invoking(x => x.AddReleaseCreatorClientServicesSingleton("token"));

        // assert
        invocation.Should().NotThrow();
    }
}
