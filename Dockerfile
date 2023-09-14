ARG RESOURCE_REAPER_SESSION_ID="00000000-0000-0000-0000-000000000000"

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
LABEL "org.testcontainers.resource-reaper-session"=$RESOURCE_REAPER_SESSION_ID
RUN apt-get -y update; apt-get -y install curl


WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG RESOURCE_REAPER_SESSION_ID="00000000-0000-0000-0000-000000000000"
LABEL "org.testcontainers.resource-reaper-session"=$RESOURCE_REAPER_SESSION_ID

WORKDIR /src
COPY ["src/Betatalks.Testcontainers.Api/Betatalks.Testcontainers.Api.csproj", "src/Betatalks.Testcontainers.Api/"]
COPY ["src/Betatalks.Testcontainers.Core/Betatalks.Testcontainers.Core.csproj", "src/Betatalks.Testcontainers.Core/"]
COPY ["src/Betatalks.Testcontainers.Infrastructure/Betatalks.Testcontainers.Infrastructure.csproj", "src/Betatalks.Testcontainers.Infrastructure/"]
RUN dotnet restore "src/Betatalks.Testcontainers.Api/Betatalks.Testcontainers.Api.csproj"
COPY . .
WORKDIR "/src/src/Betatalks.Testcontainers.Api"
RUN dotnet build "Betatalks.Testcontainers.Api.csproj" -c Release -o /app/build
RUN dotnet publish "Betatalks.Testcontainers.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

HEALTHCHECK --interval=500ms --timeout=3s --start-period=5s --retries=5 CMD [ "curl", "-f", "http://localhost:80/api/users" ]
ENTRYPOINT ["dotnet", "Betatalks.Testcontainers.Api.dll"]