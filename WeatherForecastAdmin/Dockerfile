#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.
#EXPL: generated directly from VS. It is used from command line to build the docker images to publish to the docker hub registry.
#from the directory where the .sln file is located, execute, for example:
#docker build -f "d:\experiments\saal-digital\WeatherForecastAdmin\Dockerfile" -t rethappmember1/saal-digital-weatherforecast-admin:v2 .

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["WeatherForecastAdmin/WeatherForecastAdmin.csproj", "WeatherForecastAdmin/"]
RUN dotnet restore "./WeatherForecastAdmin/WeatherForecastAdmin.csproj"
COPY . .
WORKDIR "/src/WeatherForecastAdmin"
RUN dotnet build "./WeatherForecastAdmin.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WeatherForecastAdmin.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WeatherForecastAdmin.dll"]