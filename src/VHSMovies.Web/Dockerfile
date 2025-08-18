FROM nginx AS base
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["src/VHSMovies.Web/VHSMovies.Web.csproj", "src/VHSMovies.Web/"]
COPY ["src/VHSMovies.Api.Integration/VHSMovies.Api.Integration.csproj", "src/VHSMovies.Api.Integration/"]
RUN dotnet restore "src/VHSMovies.Web/VHSMovies.Web.csproj"

COPY . .
WORKDIR "/src/src/VHSMovies.Web"
RUN dotnet build "VHSMovies.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish VHSMovies.Web.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/wwwroot .
COPY ["src/VHSMovies.Web/nginx.conf", "/etc/nginx/nginx.conf"]