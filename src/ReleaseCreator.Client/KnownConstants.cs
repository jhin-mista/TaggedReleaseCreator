namespace ReleaseCreator.Client;

/// <summary>
/// Contains known constants related to release creation.
/// </summary>
public static class KnownConstants
{
    /// <summary>
    /// Contains GitHub related constants.
    /// </summary>
    public static class GitHub
    {

        /// <summary>
        /// Contains constants related to GitHub Actions.
        /// </summary>
        public static class Action
        {
            /// <summary>
            /// Contains constants related to environment variables in GitHub Actions.
            /// </summary>
            public static class EnvironmentVariables
            {
                /// <summary>
                /// Gets the name of the environment variable that contains the repository id.
                /// </summary>
                public static string RepositoryId => "GITHUB_REPOSITORY_ID";

                /// <summary>
                /// Gets the name of the environment variable that contains the file name for the actions output.
                /// </summary>
                public static string OutputFilePath => "GITHUB_OUTPUT";
            }

            /// <summary>
            /// Gets the variable name that contains the next version.
            /// </summary>
            public static string NextVersionOutputVariableName => "next-version";
        }
    }
}