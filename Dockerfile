FROM mcr.microsoft.com/dotnet/sdk:9.0.100-rc.2 AS base
WORKDIR /app
COPY . ./
RUN dotnet publish ./src/ReleaseCreator.CommandLine/ReleaseCreator.CommandLine.csproj -c Release -o out --no-self-contained

FROM mcr.microsoft.com/dotnet/sdk:9.0.100-rc.2
# Copy binaries to the final layer
COPY --from=base /app/out /app
# Set entrypoint for the container
COPY entrypoint.sh /entrypoint.sh

LABEL maintainer="Benedict Zendel"
LABEL repository="https://github.com/jhin-mista/ReleaseCreator"
LABEL homepage="https://github.com/jhin-mista/ReleaseCreator"
LABEL com.github.actions.name="Release Creator"
LABEL com.github.actions.description="Creates a GitHub release and calculates the next semantic version number."
LABEL com.github.actions.icon="package"
LABEL com.github.actions.color="gray-dark"
LABEL org.opencontainers.image.source="https://github.com/jhin-mista/ReleaseCreator"
LABEL org.opencontainers.image.description="Creates a GitHub release and calculates the next semantic version number."
LABEL org.opencontainers.image.licenses="MIT"

RUN chmod +x entrypoint.sh
ENTRYPOINT ["/entrypoint.sh"]