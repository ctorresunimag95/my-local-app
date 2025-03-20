ARG VERSION=9.0-alpine

FROM mcr.microsoft.com/dotnet/aspnet:$VERSION AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Install curl
RUN apk update \
    && apk upgrade \
    && apk add --no-cache curl\
    && rm -rf /var/cache/apk/*

USER $APP_UID
FROM mcr.microsoft.com/dotnet/sdk:$VERSION AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Cinema.Management/Cinema.Management.csproj", "Cinema.Management/"]
RUN dotnet restore "Cinema.Management/Cinema.Management.csproj"
COPY . .
WORKDIR "/src/Cinema.Management"
RUN dotnet build "Cinema.Management.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Cinema.Management.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY Cinema.Management/entrypoint.sh .

ENTRYPOINT ["./entrypoint.sh"]