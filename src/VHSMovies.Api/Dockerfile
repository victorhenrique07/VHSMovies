# Etapa base de runtime (menor e mais segura)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:5000
ENV DOTNET_EnableDiagnostics=0
EXPOSE 5000

# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia apenas os arquivos de projeto para aproveitar o cache
COPY ["src/VHSMovies.Api/VHSMovies.Api.csproj", "src/VHSMovies.Api/"]
COPY ["src/VHSMovies.Application/VHSMovies.Application.csproj", "src/VHSMovies.Application/"]
COPY ["src/VHSMovies.Domain/VHSMovies.Domain.csproj", "src/VHSMovies.Domain/"]
COPY ["src/VHSMovies.Infrastructure/VHSMovies.Infrastructure.csproj", "src/VHSMovies.Infrastructure/"]
COPY ["src/VHSMovies.Mediator/VHSMovies.Mediator.csproj", "src/VHSMovies.Mediator/"]

# Restaura dependências com cache
RUN dotnet restore "src/VHSMovies.Api/VHSMovies.Api.csproj"

# Copia todo o código-fonte
COPY . .

# Compila o projeto
WORKDIR "/src/src/VHSMovies.Api"
RUN dotnet build "VHSMovies.Api.csproj" -c Release -o /app/build

# Etapa de publicação
FROM build AS publish
RUN dotnet publish "VHSMovies.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa final com runtime leve
FROM runtime AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VHSMovies.Api.dll"]
