# См. статью по ссылке https://aka.ms/customizecontainer, чтобы узнать как настроить контейнер отладки и как Visual Studio использует этот Dockerfile для создания образов для ускорения отладки.

# Этот этап используется при запуске из VS в быстром режиме (по умолчанию для конфигурации отладки)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 2000


# Этот этап используется для сборки проекта службы
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
RUN dotnet tool install --global dotnet-ef --version 8.0.0
ENV PATH="${PATH}:/root/.dotnet/tools"
COPY UserService.Web/UserService.Web.csproj UserService.Web/
COPY UserService.Domain/UserService.Domain.csproj UserService.Domain/
COPY UserService.Infrastructure/UserService.Infrastructure.csproj UserService.Infrastructure/
COPY UserService.Application/UserService.Application.csproj UserService.Application/
RUN dotnet restore ./UserService.Web/UserService.Web.csproj
COPY . .
RUN dotnet ef migrations add Migrate \
    --project UserService.Infrastructure \
    --startup-project UserService.Web \
    --output-dir Migrations
WORKDIR /src/UserService.Web/
RUN dotnet build "./UserService.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build


# Этот этап используется для публикации проекта службы, который будет скопирован на последний этап
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./UserService.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish --no-restore
# Этот этап используется в рабочей среде или при запуске из VS в обычном режиме (по умолчанию, когда конфигурация отладки не используется)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UserService.Web.dll"]