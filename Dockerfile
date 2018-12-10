FROM microsoft/dotnet:2.1.1-aspnetcore-runtime-stretch-slim

WORKDIR /app
COPY ./output ./

ENTRYPOINT [ "dotnet", "IAMSYNC.WebApi.dll" ]