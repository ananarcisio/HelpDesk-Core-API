# Estágio de compilação
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar os arquivos de projeto e restaurar dependências
COPY ["HelpDeskCore.Api/HelpDeskCore.Api.csproj", "HelpDeskCore.Api/"]
RUN dotnet restore "HelpDeskCore.Api/HelpDeskCore.Api.csproj"

# Copiar todo o código e compilar
COPY . .
WORKDIR "/src/HelpDeskCore.Api"
RUN dotnet publish "HelpDeskCore.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Estágio final de execução
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Definir a porta padrão exposta pelo Render
ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

ENTRYPOINT ["dotnet", "HelpDeskCore.Api.dll"]
