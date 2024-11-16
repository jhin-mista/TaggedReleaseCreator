using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace ReleaseCreator.Git.ShellExecution
{
    internal class PowerShellExecutor(ILogger<PowerShellExecutor> logger) : IPowerShellExecutor
    {
        private readonly ILogger<PowerShellExecutor> _logger = logger;

        public Collection<PSObject> Execute(string script)
        {
            var initialSessionState = InitialSessionState.CreateDefault();
            using var runSpace = RunspaceFactory.CreateRunspace(initialSessionState);

            runSpace.Open();
            var result = ExecuteScript(runSpace, script);
            runSpace.Close();

            return result;
        }

        private Collection<PSObject> ExecuteScript(Runspace runSpace, string script)
        {
            using var powerShell = PowerShell.Create(runSpace);
            powerShell.AddScript(script);

            var result = powerShell.Invoke();

            if (powerShell.Streams.Error.Count > 0)
            {
                var exceptions = powerShell.Streams.Error.Select(x => x.Exception);
                throw new AggregateException($"Error(s) executing script '{script}'", exceptions);
            }

            _logger.LogDebug("Script execution returned {result}", result);

            return result;
        }
    }
}