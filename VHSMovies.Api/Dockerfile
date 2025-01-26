# Acesse https://aka.ms/customizecontainer para saber como personalizar seu contêiner de depuração e como o Visual Studio usa este Dockerfile para criar suas imagens para uma depuração mais rápida.

# Esta fase é usada durante a execução no VS no modo rápido (Padrão para a configuração de Depuração)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# Esta fase é usada para compilar o projeto de serviço
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["VHSMovies.Api/VHSMovies.Api.csproj", "VHSMovies.Api/"]
COPY ["VHSMovies.Application/VHSMovies.Application.csproj", "VHSMovies.Application/"]
COPY ["VHSMovies.Domain/VHSMovies.Domain.csproj", "VHSMovies.Domain/"]
COPY ["VHSMovies.Infraestructure/VHSMovies.Infraestructure.csproj", "VHSMovies.Infraestructure/"]
RUN dotnet restore "./VHSMovies.Api/VHSMovies.Api.csproj"
COPY . .
WORKDIR "/src/VHSMovies.Api"
RUN dotnet build "./VHSMovies.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Esta fase é usada para publicar o projeto de serviço a ser copiado para a fase final
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./VHSMovies.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Esta fase é usada na produção ou quando executada no VS no modo normal (padrão quando não está usando a configuração de Depuração)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VHSMovies.Api.dll"]