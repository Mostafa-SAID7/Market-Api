# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ASP.MongoDb.API/ASP.MongoDb.API.csproj ASP.MongoDb.API/
RUN dotnet restore "ASP.MongoDb.API/ASP.MongoDb.API.csproj"

# Copy everything else and build
COPY . .
WORKDIR /src/ASP.MongoDb.API
RUN dotnet build "ASP.MongoDb.API.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "ASP.MongoDb.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ASP.MongoDb.API.dll"]
