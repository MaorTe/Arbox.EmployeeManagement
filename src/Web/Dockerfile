# Use the ASP.NET runtime image for .NET 9
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Use the full SDK image to build & publish
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy your solution and projects
COPY ["Arbox.EmployeeManagement.sln", "./"]
COPY ["src/Web/Web.csproj", "src/Web/"]
COPY ["src/Infrastructure/Infrastructure.csproj", "src/Infrastructure/"]
COPY ["src/Application/Application.csproj", "src/Application/"]
COPY ["src/Core/Core.csproj", "src/Core/"]
# (adjust paths if your csproj names differ)

# Restore NuGet packages
RUN dotnet restore "Arbox.EmployeeManagement.sln"

# Copy everything else and build
COPY . .
WORKDIR "/src/src/Web"
RUN dotnet build "Web.csproj" -c Release -o /app/build

# Publish a self-contained release
FROM build AS publish
RUN dotnet publish "Web.csproj" -c Release -o /app/publish

# Final stage: copy published bits into the runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Web.dll"]