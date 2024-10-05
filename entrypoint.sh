#!/bin/sh

mv /app /github/workspace/ReleaseCreator
# Make binaries executable
chmod -R +x /github/workspace/ReleaseCreator
dotnet /github/workspace/ReleaseCreator/ReleaseCreator.CommandLine.dll $@