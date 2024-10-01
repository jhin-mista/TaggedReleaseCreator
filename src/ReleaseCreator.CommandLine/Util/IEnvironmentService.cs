namespace ReleaseCreator.CommandLine.Util;

internal interface IEnvironmentService
{
    public string? GetEnvironmentVariable(string name);
}