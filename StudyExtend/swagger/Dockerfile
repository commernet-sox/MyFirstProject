#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["swagger/swagger.csproj", "swagger/"]
COPY ["swaggerOne/swaggerOne.csproj", "swaggerOne/"]
RUN dotnet nuget add source "http://10.27.1.28:1024/Nuget"
#RUN docker network connect --alias net_mq net swagger
RUN dotnet restore "swagger/swagger.csproj"
COPY . .
WORKDIR "/src/swagger"
RUN dotnet build "swagger.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "swagger.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "swagger.dll"]