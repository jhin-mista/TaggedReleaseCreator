using FluentAssertions;
using ReleaseCreator.Types;
using System.CommandLine;
using System.Reflection;

namespace ReleaseCreator.Tests;

[TestFixture]
public class ReleaseCreatorOptionsBinderTest
{
    private ReleaseCreatorOptionsBinder sut;

    [SetUp]
    public void Setup()
    {
        sut = new();
    }

    [Test]
    public void AddOptionsTo_ShouldAddAllOptionsPropertiesToCommand()
    {
        // arrange
        var command = new Command("Test");

        // act
        sut.AddOptionsTo(command);

        // assert
        var sutType = sut.GetType();
        var sutPropertiesInfo = sutType.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance);
        var expectedOptions = sutPropertiesInfo.Where(
                x => x.PropertyType.IsGenericType && x.PropertyType.GetGenericTypeDefinition() == typeof(Option<>))
            .ToArray();

        command.Options.Should().HaveCount(expectedOptions.Length);

        var actualOptions = command.Options;
        foreach (var optionPropertyInfo in expectedOptions)
        {
            var expectedOption = (Option)optionPropertyInfo.GetValue(sut)!;
            actualOptions.Should().Contain(expectedOption, $"only {nameof(Option)} properties of {nameof(ReleaseCreatorOptionsBinder)} should be added to the given command");
        }
    }
}
