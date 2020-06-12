FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build

WORKDIR /stage

COPY ./TextClassification.Contracts/*.csproj ./TextClassification.Contracts/
COPY ./TextClassification.Client/*.csproj ./TextClassification.Client/

RUN dotnet restore ./TextClassification.Client/TextClassification.Client.csproj -s https://api.nuget.org/v3/index.json

COPY . ./

RUN dotnet publish ./TextClassification.Client/TextClassification.Client.csproj --configuration Release

FROM nginx:alpine

COPY --from=build /stage/TextClassification.Client/bin/Release/netstandard2.1/publish/TextClassification.Client/dist/ ./usr/share/nginx/html/