namespace ReleaseCreator.CommandLine.Util;

internal interface IEnvironmentService
{
    public string GetRequiredEnvironmentVariable(string name);
}