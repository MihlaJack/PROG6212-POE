# Use the official ASP.NET Core runtime as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Use the SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["RaceDayWeb/RaceDayWeb.csproj", "RaceDayWeb/"]
RUN dotnet restore "RaceDayWeb/RaceDayWeb.csproj"
COPY . .
WORKDIR "/src/RaceDayWeb"
RUN dotnet build "RaceDayWeb.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "RaceDayWeb.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Use the runtime image for the final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RaceDayWeb.dll"]
