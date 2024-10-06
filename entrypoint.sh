#!/bin/sh

mv /app /github/workspace/ReleaseCreator
# Make binaries executable
chmod -R +x /github/workspace/ReleaseCreator
git config --global --add safe.directory /github/workspace
dotnet /github/workspace/ReleaseCreator/ReleaseCreator.CommandLine.dll $@