using System.Collections.ObjectModel;
using System.Management.Automation;

namespace ReleaseCreator.Git.ShellExecution;

internal interface IPowerShellExecutor
{
    public Collection<PSObject> Execute(string script);
}