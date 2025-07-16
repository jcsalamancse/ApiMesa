FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/MesaApi.Api/MesaApi.Api.csproj", "src/MesaApi.Api/"]
COPY ["src/MesaApi.Application/MesaApi.Application.csproj", "src/MesaApi.Application/"]
COPY ["src/MesaApi.Domain/MesaApi.Domain.csproj", "src/MesaApi.Domain/"]
COPY ["src/MesaApi.Infrastructure/MesaApi.Infrastructure.csproj", "src/MesaApi.Infrastructure/"]
RUN dotnet restore "src/MesaApi.Api/MesaApi.Api.csproj"
COPY . .
WORKDIR "/src/src/MesaApi.Api"
RUN dotnet build "MesaApi.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MesaApi.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MesaApi.Api.dll"]