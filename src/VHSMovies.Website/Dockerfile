FROM nginx AS base
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["src/VHSMovies.Website/VHSMovies.Website.csproj", "src/VHSMovies.Website/"]
COPY ["src/VHSMovies.Api.Integration/VHSMovies.Api.Integration.csproj", "src/VHSMovies.Api.Integration/"]
RUN dotnet restore "src/VHSMovies.Website/VHSMovies.Website.csproj"

COPY . .
WORKDIR "src/src/VHSMovies.Website"
RUN dotnet build "VHSMovies.Website.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish VHSMovies.Website.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/wwwroot .
COPY src/VHSMovies.Website/nginx.conf /etc/nginx/nginx.conf
