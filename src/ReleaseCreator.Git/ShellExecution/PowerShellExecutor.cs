using Microsoft.PowerShell.Commands;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace ReleaseCreator.Git.ShellExecution
{
    internal class PowerShellExecutor : IPowerShellExecutor
    {
        public Collection<PSObject> Execute(string script)
        {
            var initialSessionState = InitialSessionState.CreateDefault();
            using var runSpace = RunspaceFactory.CreateRunspace(initialSessionState);

            runSpace.Open();
            var result = ExecuteScript(runSpace, script);
            runSpace.Close();

            return result;
        }

        private static InitialSessionState GetInitialSessionState()
        {
            var initialSessionState = InitialSessionState.Create();

            var setLocationCmdletEntry = new SessionStateCmdletEntry("Set-Location", typeof(SetLocationCommand), null);
            var selectObjectCmdletEntry = new SessionStateCmdletEntry("Select-Object", typeof(SelectObjectCommand), null);
            var generalApplicationEntry = new SessionStateApplicationEntry("*");

            initialSessionState.Commands.Add([setLocationCmdletEntry, selectObjectCmdletEntry, generalApplicationEntry]);
            initialSessionState.LanguageMode = PSLanguageMode.FullLanguage;

            var fileSystemSessionStateProvider = new SessionStateProviderEntry("FileSystem", typeof(FileSystemProvider), null);
            var environmentSessionStateProvider = new SessionStateProviderEntry("Environment", typeof(EnvironmentProvider), null);

            initialSessionState.Providers.Add(
            [
                fileSystemSessionStateProvider,
                environmentSessionStateProvider,
            ]);

            return initialSessionState;
        }

        private static Collection<PSObject> ExecuteScript(Runspace runSpace, string script)
        {
            using var powerShell = PowerShell.Create(runSpace);
            powerShell.AddScript(script);

            var result = powerShell.Invoke();

            if (powerShell.Streams.Error.Count > 0)
            {
                var exceptions = powerShell.Streams.Error.Select(x => x.Exception);
                throw new AggregateException($"Error(s) executing script '{script}'", exceptions);
            }

            return result;
        }
    }
}