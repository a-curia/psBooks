FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["BookCovers.API/BookCovers.API.csproj", "BookCovers.API/"]
RUN dotnet restore "BookCovers.API/BookCovers.API.csproj"
COPY . .
WORKDIR "/src/BookCovers.API"
RUN dotnet build "BookCovers.API.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "BookCovers.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "BookCovers.API.dll"]