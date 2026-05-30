FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY ApiInteligenteWeb/*.csproj ./ApiInteligenteWeb/
RUN dotnet restore ./ApiInteligenteWeb/ApiInteligenteWeb.csproj

COPY ApiInteligenteWeb/. ./ApiInteligenteWeb/
RUN dotnet publish ./ApiInteligenteWeb/ApiInteligenteWeb.csproj \
    -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

ENTRYPOINT ["dotnet", "ApiInteligenteWeb.dll"]