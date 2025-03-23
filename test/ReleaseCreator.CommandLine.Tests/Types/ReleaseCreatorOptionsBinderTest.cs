using FluentAssertions;
using ReleaseCreator.CommandLine.Types;
using System.CommandLine;
using System.Reflection;

namespace ReleaseCreator.CommandLine.Tests.Types;

[TestFixture]
public class ReleaseCreatorOptionsBinderTest
{
    private ReleaseCreatorOptionsBinder _sut;

    [SetUp]
    public void Setup()
    {
        _sut = new();
    }

    [Test]
    public void AddOptionsTo_ShouldAddAllOptionsPropertiesToCommand()
    {
        // arrange
        var command = new Command("Test");

        // act
        _sut.AddOptionsTo(command);

        // assert
        var sutType = _sut.GetType();
        var sutPropertiesInfo = sutType.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance);
        var expectedOptions = sutPropertiesInfo.Where(
                x => x.PropertyType.IsGenericType && x.PropertyType.GetGenericTypeDefinition() == typeof(Option<>))
            .ToArray();

        command.Options.Should().HaveCount(expectedOptions.Length);

        var actualOptions = command.Options;
        foreach (var optionPropertyInfo in expectedOptions)
        {
            var expectedOption = (Option)optionPropertyInfo.GetValue(_sut)!;
            actualOptions.Should().Contain(expectedOption, $"only {nameof(Option)} properties of {nameof(ReleaseCreatorOptionsBinder)} should be added to the given command");
        }
    }

    [Test]
    public void Invoke_WhenPreReleaseIdentifierIsEmptyString_ShouldPassPreReleaseOptionsAsExpected()
    {
        // arrange
        var command = new Command("Test");
        _sut.AddOptionsTo(command);

        string[] arguments =
            [
                "--commit", "abc123",
                "--type", "major",
                "--token", "token",
                "--pre-release", "",
            ];

        ReleaseCreatorCommandLineOptions? releaseCreatorCommandLineOptions = null;
        command.SetHandler(x => releaseCreatorCommandLineOptions = x, _sut);

        // act
        command.Invoke(arguments);

        // assert
        releaseCreatorCommandLineOptions.Should().NotBeNull();
        releaseCreatorCommandLineOptions!.PreReleaseIdentifier.Should().BeEmpty();
        releaseCreatorCommandLineOptions.IsPreRelease.Should().BeTrue();
    }

    [Test]
    public void Invoke_WhenPreReleaseIdentifierIsNotSet_ShouldPassPreReleaseOptionsAsExpected()
    {
        // arrange
        var command = new Command("Test");
        _sut.AddOptionsTo(command);

        string[] arguments =
            [
                "--commit", "abc123",
                "--type", "major",
                "--token", "token",
                "--pre-release",
            ];

        ReleaseCreatorCommandLineOptions? releaseCreatorCommandLineOptions = null;
        command.SetHandler(x => releaseCreatorCommandLineOptions = x, _sut);

        // act
        command.Invoke(arguments);

        // assert
        releaseCreatorCommandLineOptions.Should().NotBeNull();
        releaseCreatorCommandLineOptions!.PreReleaseIdentifier.Should().BeEmpty();
        releaseCreatorCommandLineOptions.IsPreRelease.Should().BeTrue();
    }
}