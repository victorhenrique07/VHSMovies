# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5000


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["VHSMovies.Api/VHSMovies.Api.csproj", "VHSMovies.Api/"]
COPY ["VHSMovies.Application/VHSMovies.Application.csproj", "VHSMovies.Application/"]
COPY ["VHSMovies.Domain/VHSMovies.Domain.csproj", "VHSMovies.Domain/"]
COPY ["VHSMovies.Infrastructure/VHSMovies.Infrastructure.csproj", "VHSMovies.Infrastructure/"]
RUN dotnet restore "./VHSMovies.Api/VHSMovies.Api.csproj"
COPY . .
WORKDIR "/src/VHSMovies.Api"
RUN dotnet build "./VHSMovies.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./VHSMovies.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VHSMovies.Api.dll"]