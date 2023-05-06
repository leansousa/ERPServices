#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ERPServices.Report.API/ERPServices.Report.API.csproj", "ERPServices.Report.API/"]
RUN dotnet restore "ERPServices.Report.API/ERPServices.Report.API.csproj"
COPY . .
WORKDIR "/src/ERPServices.Report.API"
RUN dotnet build "ERPServices.Report.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ERPServices.Report.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ERPServices.Report.API.dll"]