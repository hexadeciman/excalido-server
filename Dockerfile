# Use the official .NET SDK for building the app
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy only the solution and required projects
COPY ["UltimateTodoServer.sln", "./"]
COPY ["Api/Api.csproj", "Api/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["Tests/Tests.csproj", "Tests/"]

# Restore dependencies
RUN dotnet restore

# Copy the full source code except Tests
COPY . .
WORKDIR /src/Api
RUN dotnet publish -c Release -o /app/out

# Use a lightweight runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

ENV ASPNETCORE_ENVIRONMENT=Production
ENV DOTNET_NOLOGO=true
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1

EXPOSE 7272

# Run the API
ENTRYPOINT ["dotnet", "Api.dll"]
