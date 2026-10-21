# Estágio de compilação
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar todos os ficheiros do projeto
COPY . .

# Restaurar e publicar a partir da pasta da API
WORKDIR /src/HelpDeskCore.Api
RUN dotnet restore "HelpDeskCore.Api.csproj"
RUN dotnet publish "HelpDeskCore.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Estágio final de execução
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Definir a porta padrão do Render
ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

ENTRYPOINT ["dotnet", "HelpDeskCore.Api.dll"]
