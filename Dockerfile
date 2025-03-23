FROM mcr.microsoft.com/dotnet/sdk:9.0 AS base

FROM base AS appbase
WORKDIR /app
COPY . ./

FROM appbase AS publish
RUN dotnet publish ./src/ReleaseCreator.CommandLine/ReleaseCreator.CommandLine.csproj -c Release -o out --no-self-contained

FROM base AS final
# Copy binaries to the final layer
COPY --from=publish /app/out /app
# Set entrypoint for the container
COPY entrypoint.sh /entrypoint.sh

LABEL maintainer="Benedict Zendel"
LABEL repository="https://github.com/jhin-mista/TaggedReleaseCreator"
LABEL homepage="https://github.com/jhin-mista/TaggedReleaseCreator"
LABEL com.github.actions.name="Tagged Release Creator"
LABEL com.github.actions.description="Creates a GitHub release and calculates the next semantic version number."
LABEL com.github.actions.icon="package"
LABEL com.github.actions.color="gray-dark"
LABEL org.opencontainers.image.source="https://github.com/jhin-mista/TaggedReleaseCreator"
LABEL org.opencontainers.image.description="Creates a GitHub release and calculates the next semantic version number."
LABEL org.opencontainers.image.licenses="MIT"

RUN chmod +x entrypoint.sh
ENTRYPOINT ["/entrypoint.sh"]