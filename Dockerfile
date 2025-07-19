FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8081
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Minimal.Presentation/Minimal.Presentation.csproj", "src/Minimal.Presentation/"]
COPY ["src/Minimal.Application/Minimal.Application.csproj", "src/Minimal.Application/"]
COPY ["src/Minimal.Domain/Minimal.Domain.csproj", "src/Minimal.Domain/"]
COPY ["src/Minimal.Infrastructure/Minimal.Infrastructure.csproj", "src/Minimal.Infrastructure/"]
COPY ["src/Minimal.Shared/Minimal.Shared.csproj", "src/Minimal.Shared/"]
COPY ["src/Minimal.Contracts/Minimal.Contracts.csproj", "src/Minimal.Contracts/"]
RUN dotnet restore "src/Minimal.Presentation/Minimal.Presentation.csproj"
COPY . .
WORKDIR "/src/src/Minimal.Presentation"
RUN dotnet build "Minimal.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Minimal.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Minimal.Presentation.dll"]
