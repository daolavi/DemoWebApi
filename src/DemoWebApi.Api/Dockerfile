FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/DemoWebApi.Api/DemoWebApi.Api.csproj", "src/DemoWebApi.Api/"]
COPY ["src/DemoWebApi.Infrastructure/DemoWebApi.Infrastructure.csproj", "src/DemoWebApi.Infrastructure/"]
COPY ["src/DemoWebApi.Domain/DemoWebApi.Domain.csproj", "src/DemoWebApi.Domain/"]
COPY ["src/DemoWebApi.Contracts/DemoWebApi.Contracts.csproj", "src/DemoWebApi.Contracts/"]
COPY ["src/DemoWebApi.Application/DemoWebApi.Application.csproj", "src/DemoWebApi.Application/"]
RUN dotnet restore "src/DemoWebApi.Api/DemoWebApi.Api.csproj"
COPY . .
WORKDIR "/src/src/DemoWebApi.Api"
RUN dotnet build "DemoWebApi.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "DemoWebApi.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DemoWebApi.Api.dll"]
