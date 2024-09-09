# Use the official ASP.NET runtime image as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Use the SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the csproj file and restore the dependencies
COPY ["OccupancyTracker/OccupancyTracker.csproj", "OccupancyTracker/"]
RUN dotnet restore "OccupancyTracker/OccupancyTracker.csproj"

# Copy the rest of the project files
COPY . .
WORKDIR "/src/OccupancyTracker"
RUN dotnet build "OccupancyTracker.csproj" -c Release -o /app/build

# Publish the app
FROM build AS publish
RUN dotnet publish "OccupancyTracker.csproj" -c Release -o /app/publish

# Use the base image to run the app
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OccupancyTracker.dll"]
