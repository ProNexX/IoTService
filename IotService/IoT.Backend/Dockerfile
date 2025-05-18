# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Only include relevant dependencies
COPY ["IoT.Backend/IoT.Backend.csproj", "IoT.Backend/"]
COPY ["IoT.DataAccess/IoT.DataAccess.csproj", "IoT.DataAccess/"]
RUN dotnet restore "IoT.Backend/IoT.Backend.csproj"

# Copy full solution and build
COPY . .
WORKDIR "/src/IoT.Backend"
RUN dotnet publish "IoT.Backend.csproj" -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Healthcheck for SignalR and API
HEALTHCHECK --interval=10s --timeout=3s --start-period=15s CMD curl --fail http://localhost/health || exit 1

# Logging and environment
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Development

ENTRYPOINT ["dotnet", "IoT.Backend.dll"]