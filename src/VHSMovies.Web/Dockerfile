# Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["src/VHSMovies.Web/VHSMovies.Web.csproj", "src/VHSMovies.Web/"]
COPY ["src/VHSMovies.Api.Integration/VHSMovies.Api.Integration.csproj", "src/VHSMovies.Api.Integration/"]
RUN dotnet restore "src/VHSMovies.Web/VHSMovies.Web.csproj"

COPY . .
WORKDIR "/src/src/VHSMovies.Web"
RUN dotnet publish "VHSMovies.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "VHSMovies.Web.dll"]
