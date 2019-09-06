FROM mcr.microsoft.com/dotnet/core/aspnet:2.2

WORKDIR /app
COPY ./output/app ./

ENTRYPOINT [ "dotnet", "AwsJanitor.WebApi.dll" ]
