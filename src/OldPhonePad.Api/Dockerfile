FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ./src/OldPhonePad.Api/*.csproj ./OldPhonePad.Api/
COPY ./src/OldPhonePad.Lib/*.csproj ./OldPhonePad.Lib/
COPY ./src/OldPhonePad.Console/*.csproj ./OldPhonePad.Console/

RUN dotnet restore ./OldPhonePad.Api/OldPhonePad.Api.csproj

COPY ./src/ ./ 

RUN dotnet publish ./OldPhonePad.Api/OldPhonePad.Api.csproj -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "OldPhonePad.Api.dll"]
