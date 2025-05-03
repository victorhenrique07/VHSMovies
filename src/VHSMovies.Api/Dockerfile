# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Base image to run the app
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000

# Build the app
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/VHSMovies.Api/VHSMovies.Api.csproj", "src/VHSMovies.Api/"]
COPY ["src/VHSMovies.Application/VHSMovies.Application.csproj", "src/VHSMovies.Application/"]
COPY ["src/VHSMovies.Domain/VHSMovies.Domain.csproj", "src/VHSMovies.Domain/"]
COPY ["src/VHSMovies.Infrastructure/VHSMovies.Infrastructure.csproj", "src/VHSMovies.Infrastructure/"]
COPY ["src/VHSMovies.Mediator/VHSMovies.Mediator.csproj", "src/VHSMovies.Mediator/"]
RUN dotnet restore "./src/VHSMovies.Api/VHSMovies.Api.csproj"
COPY . .
WORKDIR "/src/src/VHSMovies.Api"
RUN dotnet build "./VHSMovies.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the app
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./VHSMovies.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VHSMovies.Api.dll"]
