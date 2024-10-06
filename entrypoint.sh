#!/bin/sh

mv /app /github/workspace/ReleaseCreator

# Make binaries executable
chmod -R +x /github/workspace/ReleaseCreator

git config --global --add safe.directory /github/workspace
git fetch --tags

# Only take non empty arguments
for arg in "$@"
do
    if [ -n "$arg" ]; then
        arguments="${arguments} $arg";
    fi
done
set -- $arguments

git tag
echo "$@"
# dotnet /github/workspace/ReleaseCreator/ReleaseCreator.CommandLine.dll $@