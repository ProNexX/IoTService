FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy only the projects needed
COPY ["IoT.KafkaConsumer/IoT.KafkaConsumer.csproj", "IoT.KafkaConsumer/"]
COPY ["IoT.DataAccess/IoT.DataAccess.csproj", "IoT.DataAccess/"]
RUN dotnet restore "IoT.KafkaConsumer/IoT.KafkaConsumer.csproj"

# Now copy only the required folders (not whole solution)
COPY IoT.KafkaConsumer/ ./IoT.KafkaConsumer/
COPY IoT.DataAccess/ ./IoT.DataAccess/
WORKDIR /src/IoT.KafkaConsumer

RUN dotnet publish "IoT.KafkaConsumer.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

HEALTHCHECK --interval=10s --timeout=3s --start-period=15s CMD curl --fail http://localhost || exit 1

ENTRYPOINT ["dotnet", "IoT.KafkaConsumer.dll"]