FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /source

COPY *.sln .
COPY *.csproj .
RUN dotnet restore ComicRay.sln

COPY . .
WORKDIR /source
RUN dotnet publish -c release -o /app --no-restore ComicRay.sln

FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build /app ./

CMD ASPNETCORE_URLS=http://*:$PORT dotnet ComicAPI.dll