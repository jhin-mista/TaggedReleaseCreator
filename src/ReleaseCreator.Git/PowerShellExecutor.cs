using System.Management.Automation;
using System.Collections.ObjectModel;

namespace ReleaseCreator.Git
{
    internal class PowerShellExecutor : IPowerShellExecutor
    {
        public Collection<PSObject> Execute(string script)
        {
            using var powerShell = PowerShell.Create();

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