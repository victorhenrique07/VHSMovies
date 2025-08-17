FROM nginx AS base
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["VHSMovies.Web/VHSMovies.Web.csproj", "VHSMovies.Web/"]
COPY ["VHSMovies.Api.Integration/VHSMovies.Api.Integration.csproj", "VHSMovies.Api.Integration/"]
RUN dotnet restore "VHSMovies.Web/VHSMovies.Webs.csproj"

COPY . .
WORKDIR "/src/VHSMovies.Web"
RUN dotnet build "VHSMovies.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish VHSMovies.Web.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/wwwroot .
COPY VHSMovies.Web/nginx.conf /etc/nginx/nginx.conf
