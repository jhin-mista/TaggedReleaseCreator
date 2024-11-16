using System.CommandLine;
using System.CommandLine.Binding;

namespace ReleaseCreator.Types;

internal class ReleaseCreatorOptionsBinder : BinderBase<ReleaseCreatorOptions>
{
    internal Option<string> BranchNameOption { get; }
    internal Option<SemanticVersionPart> VersionIncreaseTypeOption { get; }
    internal Option<string> PreReleaseOption { get; }
    internal Option<string> AccessTokenOption { get; }

    internal ReleaseCreatorOptionsBinder()
    {
        BranchNameOption = new Option<string>("--branch", "The branch to create the release from")
        {
            IsRequired = true,
        };

        VersionIncreaseTypeOption = new Option<SemanticVersionPart>("--type", "The release type to create")
        {
            IsRequired = true,
        };

        PreReleaseOption = new Option<string>("--pre-release", "Indicates if this release is a pre-release. Optionally set a pre-release identifier")
        {
            Arity = ArgumentArity.ZeroOrOne,
        };

        AccessTokenOption = new Option<string>("--token", "The access token for authenticating against github")
        {
            IsRequired = true,
        };
    }

    internal void AddOptionsTo(Command command)
    {
        command.AddOption(BranchNameOption);
        command.AddOption(VersionIncreaseTypeOption);
        command.AddOption(PreReleaseOption);
        command.AddOption(AccessTokenOption);
    }

    /// <inheritdoc/>
    protected override ReleaseCreatorOptions GetBoundValue(BindingContext bindingContext)
    {
        return new(
            bindingContext.ParseResult.GetValueForOption(BranchNameOption)!,
            bindingContext.ParseResult.GetValueForOption(VersionIncreaseTypeOption),
            bindingContext.ParseResult.GetValueForOption(PreReleaseOption),
            bindingContext.ParseResult.GetValueForOption(AccessTokenOption)!
            );
    }
}