# Imagem base para Azure Functions com .NET
FROM mcr.microsoft.com/azure-functions/dotnet:4 AS base
WORKDIR /home/site/wwwroot
EXPOSE 80

# Estágio de build com .NET SDK
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copie todos os arquivos da solução, incluindo os projetos dependentes
COPY ["ServiceClock/ServiceClock_BackEnd.csproj", "."]
COPY ["ServiceClock_BackEnd_Application/ServiceClock_BackEnd_Application.csproj", "ServiceClock_Back_End_Application/"]
COPY ["ServiceClock_BackEnd_Domain/ServiceClock_BackEnd_Domain.csproj", "ServiceClock_Back_End_Domain/"]
COPY ["ServiceClock_BackEnd_Infra/ServiceClock_BackEnd_Infra.csproj", "ServiceClock_Back_End_Infra/"]

# Restaura as dependências do projeto principal
RUN dotnet restore "ServiceClock_BackEnd.csproj"

# Copia todo o código fonte para o contêiner
COPY . .

# Define o diretório de trabalho para a pasta principal
WORKDIR "/src"
RUN dotnet build "ServiceClock/ServiceClock_BackEnd.csproj" -c Release -o /app/build

# Publicação
FROM build AS publish
RUN dotnet publish "ServiceClock/ServiceClock_BackEnd.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Estágio final para runtime Azure Functions
FROM base AS final
WORKDIR /home/site/wwwroot
COPY --from=publish /app/publish .

# Define variáveis de ambiente para o runtime Azure Functions
ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true 

# Define o entrypoint para iniciar o host de funções
ENTRYPOINT ["dotnet", "Microsoft.Azure.WebJobs.Extensions.Http.dll"]
