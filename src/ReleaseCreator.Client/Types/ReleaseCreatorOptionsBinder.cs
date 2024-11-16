using ReleaseCreator.Client.Enums;
using System.CommandLine;
using System.CommandLine.Binding;
using System.CommandLine.Parsing;

namespace ReleaseCreator.Client.Types;

internal class ReleaseCreatorOptionsBinder : BinderBase<ReleaseCreatorOptions>
{
    internal Option<string> CommitShaOption { get; }
    internal Option<ReleaseType> ReleaseTypeOption { get; }
    internal Option<string?> PreReleaseOption { get; }
    internal Option<string> AccessTokenOption { get; }

    internal ReleaseCreatorOptionsBinder()
    {
        CommitShaOption = new Option<string>("--commit", "The commit SHA to create the release from")
        {
            IsRequired = true,
        };

        ReleaseTypeOption = new Option<ReleaseType>("--type", "The release type to create")
        {
            IsRequired = true,
        };

        PreReleaseOption = new Option<string?>("--pre-release", parseArgument: ParsePreReleaseOption, description: "Indicates if this release is a pre-release. Optionally set a pre-release identifier")
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
        command.AddOption(CommitShaOption);
        command.AddOption(ReleaseTypeOption);
        command.AddOption(PreReleaseOption);
        command.AddOption(AccessTokenOption);
    }

    /// <inheritdoc/>
    protected override ReleaseCreatorOptions GetBoundValue(BindingContext bindingContext)
    {
        return new(
            bindingContext.ParseResult.GetValueForOption(CommitShaOption)!,
            bindingContext.ParseResult.GetValueForOption(ReleaseTypeOption),
            bindingContext.ParseResult.GetValueForOption(PreReleaseOption),
            bindingContext.ParseResult.GetValueForOption(AccessTokenOption)!
            );
    }

    private string? ParsePreReleaseOption(ArgumentResult argument)
    {
        return argument.Tokens.Count switch
        {
            // By default, a token count of 0 would return null.
            // Setting this to not null helps us differentiate between the pre-release option
            // being set with (token value) or without an identifier (empty string)
            // or not at all (null)
            0 => string.Empty,
            _ => argument.Tokens[0].Value,
        };
    }
}