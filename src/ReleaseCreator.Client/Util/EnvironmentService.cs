namespace ReleaseCreator.Client.Util;

internal class EnvironmentService : IEnvironmentService
{
    /// <exception cref="Exception"></exception>
    public string GetRequiredEnvironmentVariable(string name)
    {
        return Environment.GetEnvironmentVariable(name)
        ?? throw new Exception($"Environment variable '{name}' is not set but required.");
    }
}