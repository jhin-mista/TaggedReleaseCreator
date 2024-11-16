using System.Collections.ObjectModel;
using System.Management.Automation;

namespace ReleaseCreator.Git
{
    internal interface IPowerShellExecutor
    {
        public Collection<PSObject> Execute(string script);
    }
}