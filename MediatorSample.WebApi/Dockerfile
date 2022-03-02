#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["MediatorSample.WebApi/MediatorSample.WebApi.csproj", "MediatorSample.WebApi/"]
RUN dotnet restore "MediatorSample.WebApi/MediatorSample.WebApi.csproj"
COPY . .
WORKDIR "/src/MediatorSample.WebApi"
RUN dotnet build "MediatorSample.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MediatorSample.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MediatorSample.WebApi.dll"]