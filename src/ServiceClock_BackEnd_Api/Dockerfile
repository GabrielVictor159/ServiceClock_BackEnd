#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ServiceClock_BackEnd_Api/ServiceClock_BackEnd_Api.csproj", "ServiceClock_BackEnd_Api/"]
COPY ["ServiceClock_BackEnd_Application/ServiceClock_BackEnd_Application.csproj", "ServiceClock_BackEnd_Application/"]
COPY ["ServiceClock_BackEnd_Domain/ServiceClock_BackEnd_Domain.csproj", "ServiceClock_BackEnd_Domain/"]
COPY ["ServiceClock_BackEnd_Infra/ServiceClock_BackEnd_Infra.csproj", "ServiceClock_BackEnd_Infra/"]
RUN dotnet restore "ServiceClock_BackEnd_Api/ServiceClock_BackEnd_Api.csproj"
COPY . .
WORKDIR "/src/ServiceClock_BackEnd_Api"
RUN dotnet build "ServiceClock_BackEnd_Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ServiceClock_BackEnd_Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ServiceClock_BackEnd_Api.dll"]