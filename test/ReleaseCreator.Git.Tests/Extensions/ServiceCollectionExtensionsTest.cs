using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using ReleaseCreator.Git.Extensions;
using ReleaseCreator.Git.Tag;

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
        var result = _testee.AddGitServicesSingleton();

        // assert
        var provider = result.BuildServiceProvider();
        provider.Invoking(x => x.GetRequiredService<ITagRetriever>()).Should().NotThrow();
    }
}