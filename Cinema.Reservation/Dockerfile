FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Cinema.Reservation/Cinema.Reservation.csproj", "Cinema.Reservation/"]
RUN dotnet restore "Cinema.Reservation/Cinema.Reservation.csproj"
COPY . .
WORKDIR "/src/Cinema.Reservation"
RUN dotnet build "Cinema.Reservation.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Cinema.Reservation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY Cinema.Reservation/entrypoint.sh .

ENTRYPOINT ["./entrypoint.sh"]
