FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 6000
EXPOSE 6001



FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["OrchestratedSagaApp.csproj", "./"]
RUN dotnet restore "OrchestratedSagaApp.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "OrchestratedSagaApp.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "OrchestratedSagaApp.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrchestratedSagaApp.dll"]
