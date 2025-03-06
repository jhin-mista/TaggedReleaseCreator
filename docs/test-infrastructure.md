# Test Infrastructure

## What tests are executed
All the tests are executed. Additionally, the Dockerfile is tested by running a docker build on it.

## Where tests are executed
The tests run inside a docker container that has been built with the same image that is being used in production. The container is hosted by a GitHub action runner.

## When tests are executed
All tests are executed when a PR gets opened and on every subsequent push to the respective branch.
The tests are also executed when a branch got merged into main.

## How tests are executed
There is a [workflow file](/.github/workflows/ci.yml) that is responsible for spinning up the action runner and executing all the required commands for running the tests.
The workflow file mainly calls `docker compose` with different arguments. The respective compose file can be found [here](/test/compose.yaml).
Each command required for executing the tests is defined in its own service. This was done to not have all the commands executed in a single workflow step. This will make searching through runner logs easier as they are also separated by step

Each `docker compose run` command creates its own container. The steps later in the chain have a dependency on the files created by their predecessors. To share the files, the compose file defines two volumes. One for the nuget cache, and the other for the source directory in which the build artifacts will be generated.

To ensure the correct image is used, a pull policies have been configured to never pull an image from a remote registry.

Only the first service in the chain is building the image. The rest require the image to already be present.

The service that builds the image uses the [Dockerfile](/Dockerfile) used in production but builds the image from the `appbase` stage, skipping the included publish to have more control over it in the compose file. Doing this ensures that the base image for prod and tests are always the same.