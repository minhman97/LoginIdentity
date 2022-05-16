FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["LoginIdentity/LoginIdentity.csproj", "LoginIdentity/"]
RUN dotnet restore "LoginIdentity/LoginIdentity.csproj"
COPY . .
WORKDIR "/src/LoginIdentity"
RUN dotnet build "LoginIdentity.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LoginIdentity.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LoginIdentity.dll"]
