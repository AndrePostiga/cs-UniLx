# =================== #
#  BUILD APPLICATION  #
# =================== #
FROM mcr.microsoft.com/dotnet/sdk:9.0-noble AS build
WORKDIR "/app"

COPY ["./src/UniLx.ApiService/UniLx.ApiService.csproj" , "UniLx.ApiService/"]
COPY ["./src/UniLx.Application/UniLx.Application.csproj" , "UniLx.Application/"]
COPY ["./src/UniLx.Domain/UniLx.Domain.csproj" , "UniLx.Domain/"]
COPY ["./src/UniLx.Infra.Data/UniLx.Infra.Data.csproj" , "UniLx.Infra.Data/"]
COPY ["./src/UniLx.Infra.Services/UniLx.Infra.Services.csproj" , "UniLx.Infra.Services/"]
COPY ["./src/UniLx.ServiceDefaults/UniLx.ServiceDefaults.csproj" , "UniLx.ServiceDefaults/"]
COPY ["./src/UniLx.Shared/UniLx.Shared.csproj" , "UniLx.Shared/"]
#COPY ["./.nuget/NuGet.Config", ""]
# --configfile NuGet.Config
RUN dotnet restore "UniLx.ApiService/UniLx.ApiService.csproj" 
COPY ["src", ""]
RUN dotnet build "UniLx.ApiService/UniLx.ApiService.csproj" -c Release -o "/app/build"

# ====================== #
#  PUBLISH APPLICATION   #
# ====================== #
FROM build AS publish
WORKDIR "/app"
RUN dotnet publish --no-restore "UniLx.ApiService/UniLx.ApiService.csproj" -c Release -o "/app/publish" /p:UseAppHost=false

# ===================================== #
#  RUN APPLICATION WITH NONROOTUSER ON  #
#  DISTROLESS IMAGE                     #
# ===================================== #
FROM mcr.microsoft.com/dotnet/aspnet:9.0-noble-chiseled-extra
WORKDIR "/app"
COPY --from=publish /app/publish .

# Default Environment
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false 
ENV ASPNETCORE_URLS="http://*:3000"
EXPOSE 3000
USER $APP_UID
ENTRYPOINT ["dotnet", "UniLx.ApiService.dll"]

