# syntax=docker/dockerfile:1

# USE THIS ONLY WHEN BUILDING THE C# APPLICATION INSIDE A DOCKER CONTAINER

FROM mcr.microsoft.com/dotnet/sdk:7.0 as builder

RUN apt update && apt install openssl ca-certificates
# since we need to fetch external packages from nuget to build the application

WORKDIR /src

COPY . .

RUN dotnet restore && dotnet publish -v d --self-contained --os linux --configuration Release

FROM ubuntu:22.04

COPY --from=builder /src/bin/Release/net7.0/linux-x64/publish /app

WORKDIR /app

RUN chmod +x Simple-db-crud 
# && mv /app/appsettings.json /appsettings.json
# dotnet runs the command from /, it expects appsettings.json to be at / itself. 

ENV ASPNETCORE_URLS="http://0.0.0.0:5000"

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1

CMD ["/app/Simple-db-crud"]