#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/StarWars.RestAPI/StarWars.RestAPI.csproj", "src/StarWars.RestAPI/"]
COPY ["src/StarWars.Infra.Data/StarWars.Infra.Data.csproj", "src/StarWars.Infra.Data/"]
COPY ["src/StarWars.Domain/StarWars.Domain.csproj", "src/StarWars.Domain/"]
COPY ["src/StarWars.Infra.IoC.RestAPI/StarWars.Infra.IoC.RestAPI.csproj", "src/StarWars.Infra.IoC.RestAPI/"]
RUN dotnet restore "src/StarWars.RestAPI/StarWars.RestAPI.csproj"
COPY . .
WORKDIR "/src/src/StarWars.RestAPI"
RUN dotnet build "StarWars.RestAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StarWars.RestAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StarWars.RestAPI.dll"]