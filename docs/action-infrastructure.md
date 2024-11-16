# Infrastructure Setup for the Release Creator Action
This file is intended to present an overview of the special setup needed to make the release creator action work. The infrastructure setup mainly resides in the following files
- [Dockerfile](https://github.com/jhin-mista/ReleaseCreator/blob/main/Dockerfile)
  - Creates an image that can run a dotnet application
- [action.yml](https://github.com/jhin-mista/ReleaseCreator/blob/feature/github-action-support/action.yml)
  - Metadata file defining inputs and how those inputs are being passed to the container
- [entrypoint.sh](https://github.com/jhin-mista/ReleaseCreator/blob/feature/github-action-support/entrypoint.sh)
  - Configures container at runtime and starts the release creator

## Dockerfile
The image for the container is based on the [dotnet sdk image](https://github.com/dotnet/dotnet-docker/blob/main/README.sdk.md). The runtime image cannot be used as this action depends on some functionality (PowerShell, git, etc.) only present in the sdk image.

## action.yml
To not pollute the [ReleaseCreatorOptions](https://github.com/jhin-mista/ReleaseCreator/blob/main/src/ReleaseCreator.CommandLine/Types/ReleaseCreatorOptions.cs) type with logging configuration, the log level is passed as an environment variable. The log level defaults to `LogLevel.Information`.

Right now, the container gets built every time the action is called. That should be changed in the future by fixing the respective [issue](https://github.com/jhin-mista/ReleaseCreator/issues/20).

## entrypoint.sh
Final workspace preparation and argument sanitization happens here.

The checked out and mounted repository neeeds be prepared for the usage of git inside the release creator. It needs to be ensured that all tags of the branch are fetched. To get the latest highest [semantic version](https://semver.org) correctly, the `versionsort.suffix` git config needs to be set to `-`. This will order pre-release versions after stable versions.

The action.yml passes `null` as an empty string to the container. If the release creator is called with the arguments `--pre-release` and `""` (empty string), then all previously existing pre-release identifiers of the current version would get lost as they would be replaced by the empty string. `entrypoint.sh` removes all passed arguments that are just the empty string.