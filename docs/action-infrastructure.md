# Infrastructure Setup for the Release Creator Action
This file is intended to present an overview of the special setup needed to make the release creator action work. The infrastructure setup mainly resides in the following files
- [Dockerfile](https://github.com/jhin-mista/ReleaseCreator/blob/main/Dockerfile)
  - Creates an image that can run the release creator application
- [action.yml](https://github.com/jhin-mista/ReleaseCreator/blob/feature/github-action-support/action.yml)
  - GitHub specific metadata file defining inputs and how those inputs are being passed to the container
- [entrypoint.sh](https://github.com/jhin-mista/ReleaseCreator/blob/feature/github-action-support/entrypoint.sh)
  - Entrypoint of the container
  - Configures the container at runtime and starts the release creator

## Dockerfile
The image for the container is based on the [dotnet sdk image](https://github.com/dotnet/dotnet-docker/blob/main/README.sdk.md). The runtime image cannot be used as this action depends on some functionality (PowerShell, git, etc.) only present in the sdk image.

## action.yml

### Logging
To not pollute the [ReleaseCreatorOptions](https://github.com/jhin-mista/ReleaseCreator/blob/main/src/ReleaseCreator.CommandLine/Types/ReleaseCreatorOptions.cs) type with logging configuration, the log level is passed as an environment variable. The log level defaults to `LogLevel.Information`.

### Using a pre-built container from a registry
Pulling a built container of the action would make it overly complicated to support using a specific version of the action. Pushing the container to a registry decouples the repository code state with the container code state.

Suppose the repository and container has a latest version of `1.2.3`. If `action.yml` does not reference this exact semantic version tag, it would be possible to call the `action@v1.0.0` from a workflow but the container that is actually pulled would be at version `1.2.3`.

Using tags on the container images would solve this. However, a released version and the used container version would still be decoupled as the container version tag would have to be hard coded into the `action.yml` and updated before every release. This would also defeat the purpose of automatically incrementing the semantic version as one would have to compute the next semantic version by themself before the release workflow runs. This seems very error prone.

For rolling versions to be supported, a branch for every version would have to be created. There are challenges to this approach which are described in more detail in the [roadmap](https://github.com/jhin-mista/ReleaseCreator/issues/4)

The benefit of not having to build the image on every action run does not seem worth the effort. Building the image takes roughly 30 seconds. Creating a release is not that time critical most of the time.

## entrypoint.sh
Final workspace preparation and argument sanitization happens here.

The checked out and mounted repository neeeds be prepared for the usage of git inside the release creator. It needs to be ensured that all tags of the branch are fetched. To get the latest highest [semantic version](https://semver.org) correctly, the `versionsort.suffix` git config needs to be set to `-`. This will order pre-release versions after stable versions.

The action.yml passes `null` as an empty string to the container. If the release creator is called with the arguments `--pre-release` and `""` (empty string), then all previously existing pre-release identifiers of the current version would get lost as they would be replaced by the empty string. `entrypoint.sh` removes all passed arguments that are just the empty string.