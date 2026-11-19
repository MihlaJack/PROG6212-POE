# Build stage for API
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS api-build
WORKDIR /src
COPY ["RaceDayAPI/RaceDayAPI.csproj", "RaceDayAPI/"]
RUN dotnet restore "RaceDayAPI/RaceDayAPI.csproj"
COPY . .
WORKDIR "/src/RaceDayAPI"
RUN dotnet build "RaceDayAPI.csproj" -c Release -o /app/build

# Publish API
FROM api-build AS api-publish
RUN dotnet publish "RaceDayAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Build stage for MVC
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS web-build
WORKDIR /src
COPY ["RaceDayWeb/RaceDayWeb.csproj", "RaceDayWeb/"]
RUN dotnet restore "RaceDayWeb/RaceDayWeb.csproj"
COPY . .
WORKDIR "/src/RaceDayWeb"
RUN dotnet build "RaceDayWeb.csproj" -c Release -o /app/build

# Publish MVC
FROM web-build AS web-publish
RUN dotnet publish "RaceDayWeb.csproj" -c Release -o /app/publish /p:UseAppHost=false

# API runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS api-runtime
WORKDIR /app
COPY --from=api-publish /app/publish .
EXPOSE 5201
ENTRYPOINT ["dotnet", "RaceDayAPI.dll"]

# MVC runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS web-runtime
WORKDIR /app
COPY --from=web-publish /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "RaceDayWeb.dll"]
