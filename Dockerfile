FROM mcr.microsoft.com/dotnet/sdk:9.0.100-rc.1 AS base
WORKDIR /app
COPY . ./
RUN dotnet publish ./src/ReleaseCreator.CommandLine/ReleaseCreator.CommandLine.csproj -c Release -o out --no-self-contained

LABEL maintainer="Benedict Zendel"
LABEL repository="https://github.com/jhin-mista/ReleaseCreator"
LABEL homepage="https://github.com/jhin-mista/ReleaseCreator"
LABEL com.github.actions.name="Release Creator"
LABEL com.github.actions.description="Creates a GitHub release"
LABEL com.github.actions.icon="package"
LABEL com.github.actions.color="gray-dark"

FROM mcr.microsoft.com/dotnet/sdk:9.0.100-rc.1
# Copy binaries to the final layer
COPY --from=base /app/out /github/workspace/ReleaseCreator
# Make binaries executable
RUN chmod -R +x /github/workspace/ReleaseCreator
# Set entrypoint for the container
ENTRYPOINT [ "dotnet", "/github/workspace/ReleaseCreator/ReleaseCreator.CommandLine.dll" ]