FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src
COPY ["SawyersCodeBlog/SawyersCodeBlog.csproj", "SawyersCodeBlog/"]
COPY ["SawyersCodeBlog.Client/SawyersCodeBlog.Client.csproj", "SawyersCodeBlog.Client/"]
RUN dotnet restore "SawyersCodeBlog/SawyersCodeBlog.csproj"
COPY . .
WORKDIR "/src/SawyersCodeBlog"
RUN dotnet build "SawyersCodeBlog.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SawyersCodeBlog.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SawyersCodeBlog.dll"]
