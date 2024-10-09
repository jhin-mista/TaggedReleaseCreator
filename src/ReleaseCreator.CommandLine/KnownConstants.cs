namespace ReleaseCreator.CommandLine
{
    internal static class KnownConstants
    {
        internal static class GitHub
        {
            internal static class EnvironmentVariables
            {
                internal static string RepositoryId => "GITHUB_REPOSITORY_ID";

                internal static string OutputFilePath => "GITHUB_OUTPUT";
            }

            internal static class Action
            {
                internal static string NextVersionOutputVariableName => "next-version";
            }
        }
    }
}
