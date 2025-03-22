using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using ReleaseCreator.Git.Extensions;

namespace ReleaseCreator.Git.Tests.Extensions;

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
        var invocation = _testee.Invoking(x => x.AddGitServicesSingleton());

        // assert
        invocation.Should().NotThrow();
    }
}
