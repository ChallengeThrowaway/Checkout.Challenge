#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 6000
ENV ASPNETCORE_URLS=http://+:6000

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["AcquiringBank.Mock/AcquiringBank.Mock.csproj", "AcquiringBank.Mock/"]
RUN dotnet restore "AcquiringBank.Mock/AcquiringBank.Mock.csproj"
COPY . .
WORKDIR "/src/AcquiringBank.Mock"
RUN dotnet build "AcquiringBank.Mock.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AcquiringBank.Mock.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AcquiringBank.Mock.dll"]