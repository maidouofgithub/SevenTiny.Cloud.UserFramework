FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["../10-Code/SevenTiny.Cloud.UserFramework.Api/SevenTiny.Cloud.UserFramework.Api.csproj", "../10-Code/SevenTiny.Cloud.UserFramework.Api/"]
RUN dotnet restore "../10-Code/SevenTiny.Cloud.UserFramework.Api/SevenTiny.Cloud.UserFramework.Api.csproj"
COPY . .
WORKDIR "/src/../10-Code/SevenTiny.Cloud.UserFramework.Api"
RUN dotnet build "SevenTiny.Cloud.UserFramework.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "SevenTiny.Cloud.UserFramework.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "SevenTiny.Cloud.UserFramework.Api.dll"]