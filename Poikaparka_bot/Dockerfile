FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Poikaparka_bot/Poikaparka_bot.csproj", "Poikaparka_bot/"]
RUN dotnet restore "Poikaparka_bot/Poikaparka_bot.csproj"
COPY . .
WORKDIR "/src/Poikaparka_bot"
RUN dotnet build "Poikaparka_bot.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Poikaparka_bot.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Poikaparka_bot.dll"]
