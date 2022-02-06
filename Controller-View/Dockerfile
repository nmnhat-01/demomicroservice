#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Controller-View/Controller-View.csproj", "Controller-View/"]
RUN dotnet restore "Controller-View/Controller-View.csproj"
COPY . .
WORKDIR "/src/Controller-View"
RUN dotnet build "Controller-View.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Controller-View.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Controller-View.dll"]