# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy project files
COPY ["PickleballClub.csproj", "."]

# Restore dependencies
RUN dotnet restore "PickleballClub.csproj"

# Copy source code
COPY . .

# Build the application
RUN dotnet build "PickleballClub.csproj" -c Release -o /app/build

# Publish
RUN dotnet publish "PickleballClub.csproj" -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Install curl for health checks
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Copy published application from build stage
COPY --from=build /app/publish .

# Expose ports
EXPOSE 80 443 5003

# Set environment variables
ENV ASPNETCORE_URLS=http://+:5003
ENV ASPNETCORE_ENVIRONMENT=Production

# Health check
HEALTHCHECK --interval=30s --timeout=10s --start-period=5s --retries=3 \
    CMD curl -f http://localhost:5003/health || exit 1

# Start application
ENTRYPOINT ["dotnet", "PickleballClub.dll"]
