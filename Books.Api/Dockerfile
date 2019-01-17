FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["Books.Api/Books.Api.csproj", "Books.Api/"]
RUN dotnet restore "Books.Api/Books.Api.csproj"
COPY . .
WORKDIR "/src/Books.Api"
RUN dotnet build "Books.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Books.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Books.Api.dll"]