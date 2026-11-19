# RaceDay Application MVC frontend - multi-stage Docker build

# Build stage: restore and publish the MVC app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["RaceDayWeb/RaceDayWeb.csproj", "RaceDayWeb/"]
RUN dotnet restore "RaceDayWeb/RaceDayWeb.csproj"
COPY . .
WORKDIR "/src/RaceDayWeb"
RUN dotnet publish "RaceDayWeb.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage: lean ASP.NET Core image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
ENV ASPNETCORE_HTTP_PORTS=80
COPY --from=build /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "RaceDayWeb.dll"]
