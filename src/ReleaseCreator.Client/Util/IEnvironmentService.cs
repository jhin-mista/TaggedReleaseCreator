namespace ReleaseCreator.Client.Util;

internal interface IEnvironmentService
{
    public string GetRequiredEnvironmentVariable(string name);
}