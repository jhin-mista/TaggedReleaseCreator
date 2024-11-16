namespace ReleaseCreator.CommandLine.Util;

internal class EnvironmentService : IEnvironmentService
{
    public string? GetEnvironmentVariable(string name) => Environment.GetEnvironmentVariable(name);
}