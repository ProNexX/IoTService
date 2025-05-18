FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["IoT.BlazorClient/IoT.BlazorClient.csproj", "IoT.BlazorClient/"]
RUN dotnet restore "IoT.BlazorClient/IoT.BlazorClient.csproj"

COPY . .
WORKDIR "/src/IoT.BlazorClient"
RUN dotnet publish "IoT.BlazorClient.csproj" -c Release -o /app/publish

FROM nginx:alpine
COPY --from=build /app/publish/wwwroot /usr/share/nginx/html

HEALTHCHECK --interval=10s --timeout=3s --start-period=15s CMD curl --fail http://localhost || exit 1