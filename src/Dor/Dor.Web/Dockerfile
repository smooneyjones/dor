FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Dor/Dor/Dor.csproj", "Dor/Dor/"]
RUN dotnet restore "Dor/Dor/Dor.csproj"
COPY . .
WORKDIR "/src/Dor/Dor"
RUN dotnet build "Dor.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Dor.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Dor.dll"]
