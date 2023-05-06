#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ERPServices.CashFlow.API/ERPServices.CashFlow.API.csproj", "ERPServices.CashFlow.API/"]
COPY ["ERPServices.MessageBus/ERPServices.MessageBus.csproj", "ERPServices.MessageBus/"]
RUN dotnet restore "ERPServices.CashFlow.API/ERPServices.CashFlow.API.csproj"
COPY . .
WORKDIR "/src/ERPServices.CashFlow.API"
RUN dotnet build "ERPServices.CashFlow.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ERPServices.CashFlow.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ERPServices.CashFlow.API.dll"]