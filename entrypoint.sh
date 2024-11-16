#!/bin/sh

# Make binaries executable
chmod -R +x /app

git config --global --add safe.directory /github/workspace
git fetch --tags --unshallow
git config versionsort.suffix "-"

# Only take non empty arguments
for arg in "$@"
do
    if [ -n "$arg" ]; then
        arguments="${arguments} $arg";
    fi
done
set -- $arguments

dotnet /app/ReleaseCreator.Client.dll $@