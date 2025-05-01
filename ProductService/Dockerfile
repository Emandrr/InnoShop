# См. статью по ссылке https://aka.ms/customizecontainer, чтобы узнать как настроить контейнер отладки и как Visual Studio использует этот Dockerfile для создания образов для ускорения отладки.

# Этот этап используется при запуске из VS в быстром режиме (по умолчанию для конфигурации отладки)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 3000


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
RUN dotnet tool install --global dotnet-ef --version 8.0.0
ENV PATH="${PATH}:/root/.dotnet/tools"
COPY ProductService.Web/ProductService.Web.csproj ProductService.Web/
COPY ProductService.Domain/ProductService.Domain.csproj ProductService.Domain/
COPY ProductService.Infrastructure/ProductService.Infrastructure.csproj ProductService.Infrastructure/
COPY ProductService.Application/ProductService.Application.csproj ProductService.Application/
RUN dotnet restore "./ProductService.Web/ProductService.Web.csproj"
COPY . .
RUN dotnet ef migrations add Migrate \
    --project ProductService.Infrastructure \
    --startup-project ProductService.Web \
    --output-dir Migrations
WORKDIR "/src/ProductService.Web"
RUN dotnet build "./ProductService.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Этот этап используется для публикации проекта службы, который будет скопирован на последний этап
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ProductService.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish

# Этот этап используется в рабочей среде или при запуске из VS в обычном режиме (по умолчанию, когда конфигурация отладки не используется)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProductService.Web.dll"]