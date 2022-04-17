#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["WebUi1/WebUi1.csproj", "WebUi1/"]
RUN dotnet restore "WebUi1/WebUi1.csproj"
COPY . .
WORKDIR "/src/WebUi1"
RUN dotnet build "WebUi1.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebUi1.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebUi1.dll"]
