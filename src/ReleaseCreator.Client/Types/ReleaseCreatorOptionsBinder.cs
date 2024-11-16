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
        var parsedPreReleaseIdentifier = bindingContext.ParseResult.GetValueForOption(PreReleaseOption);
        // replace empty string with null so that pre-release identifiers in the current version will not get overridden
        var preReleaseIdentifier = parsedPreReleaseIdentifier == string.Empty ? null : parsedPreReleaseIdentifier;
        var isPreRelease = parsedPreReleaseIdentifier != null;

        return new(
            bindingContext.ParseResult.GetValueForOption(CommitShaOption)!,
            bindingContext.ParseResult.GetValueForOption(ReleaseTypeOption),
            preReleaseIdentifier,
            isPreRelease,
            bindingContext.ParseResult.GetValueForOption(AccessTokenOption)!
            );
    }

    private string? ParsePreReleaseOption(ArgumentResult argument)
    {
        return argument.Tokens.Count switch
        {
            0 => null,
            _ => argument.Tokens[0].Value,
        };
    }
}