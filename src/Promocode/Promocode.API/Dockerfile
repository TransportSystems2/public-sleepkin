FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/PromoCode/PromoCode.API/PromoCode.API.csproj", "PromoCode.API/"]
RUN dotnet restore "PromoCode.API/PromoCode.API.csproj"
COPY ["src/PromoCode/PromoCode.API/", "PromoCode.API/"]
WORKDIR "/src/PromoCode.API"
RUN dotnet build "PromoCode.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PromoCode.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PromoCode.API.dll"]
