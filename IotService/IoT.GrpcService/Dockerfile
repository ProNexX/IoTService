FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["IoT.GrpcService/IoT.GrpcService.csproj", "IoT.GrpcService/"]
COPY ["IoT.KafkaProducer/IoT.KafkaProducer.csproj", "IoT.KafkaProducer/"]
RUN dotnet restore "IoT.GrpcService/IoT.GrpcService.csproj"

COPY . .
WORKDIR "/src/IoT.GrpcService"
RUN dotnet publish "IoT.GrpcService.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

HEALTHCHECK --interval=10s --timeout=3s --start-period=15s CMD curl --fail http://localhost || exit 1

ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Development

ENTRYPOINT ["dotnet", "IoT.GrpcService.dll"]