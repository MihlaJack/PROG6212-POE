# RaceDay Application API - multi-stage Docker build

# Build stage: restore and publish the API
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["RaceDayAPI/RaceDayAPI.csproj", "RaceDayAPI/"]
RUN dotnet restore "RaceDayAPI/RaceDayAPI.csproj"
COPY . .
WORKDIR "/src/RaceDayAPI"
RUN dotnet publish "RaceDayAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage: lean ASP.NET Core image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
ENV ASPNETCORE_HTTP_PORTS=80
COPY --from=build /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "RaceDayAPI.dll"]
