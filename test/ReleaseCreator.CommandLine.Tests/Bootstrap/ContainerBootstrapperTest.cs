using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using ReleaseCreator.CommandLine.Bootstrap;
using System.Collections;
using System.Reflection;

namespace ReleaseCreator.CommandLine.Tests.Bootstrap;

[TestFixture]
public class ContainerBootstrapperTest
{
    [Test]
    public void BuildUp_ShouldConfigureLogLevelByEnvironmentVariable()
    {
        // arrange
        var logLevel = LogLevel.Trace;

        Environment.SetEnvironmentVariable("Logging__LogLevel__Default", logLevel.ToString());

        // act
        var container = ContainerBootstrapper.BuildUp("token");

        // assert
        var logger = container.GetRequiredService<ILogger<ContainerBootstrapperTest>>();

        logger.IsEnabled(logLevel).Should().BeTrue();
    }

    [Test]
    public void BuildUp_ShouldContainConfiguredConsoleLogger()
    {
        // act
        var container = ContainerBootstrapper.BuildUp("token");

        // assert
        var loggerFactory = container.GetRequiredService<ILoggerFactory>();

        IsConsoleLoggerConfigured(loggerFactory).Should().BeTrue();
    }

    private static bool IsConsoleLoggerConfigured(ILoggerFactory loggerFactory)
    {
        var loggingProviderRegistrations = GetLoggingProviderRegistrations(loggerFactory);
        if (loggingProviderRegistrations is IEnumerable providerRegistrations)
        {
            foreach (var providerRegistration in providerRegistrations)
            {
                var fieldInfo = providerRegistration.GetType().GetFields().First(x => x.FieldType.Name == typeof(ILoggerProvider).Name);
                var provider = fieldInfo.GetValue(providerRegistration);

                if (provider is ConsoleLoggerProvider)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private static object? GetLoggingProviderRegistrations(ILoggerFactory loggerFactory)
    {
        return loggerFactory
            .GetType()
            .GetField("_providerRegistrations", BindingFlags.NonPublic | BindingFlags.Instance)?
            .GetValue(loggerFactory);
    }
}