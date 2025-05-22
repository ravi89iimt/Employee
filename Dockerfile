# ----------- Base Runtime Image (Alpine) -----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base

# Switch to root to install packages
USER root

# Install TLS support and add a non-root user
RUN apk update && \
    apk add --no-cache openssl ca-certificates su-exec && \
    adduser -D -s /bin/sh app

WORKDIR /app
EXPOSE 9090

# Switch to non-root user
USER app


# ----------- Build Stage -----------
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy project files
COPY ["EmployeeProject.API/EmployeeProject.API.csproj", "EmployeeProject.API/"]
COPY ["EmployeeProject.Services/EmployeeProject.Services.csproj", "EmployeeProject.Services/"]
COPY ["EmployeeProject.Model/EmployeeProject.Model.csproj", "EmployeeProject.Model/"]

# Restore dependencies
RUN dotnet restore "EmployeeProject.API/EmployeeProject.API.csproj"

# Copy the rest of the source
COPY . .

# Build the app
WORKDIR "/src/EmployeeProject.API"
RUN dotnet build "EmployeeProject.API.csproj" -c $BUILD_CONFIGURATION -o /app/build


# ----------- Publish Stage -----------
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR /src/EmployeeProject.API
RUN dotnet publish "EmployeeProject.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false


# ----------- Final Stage (Alpine) -----------
FROM base AS final
WORKDIR /app

# Copy published output
COPY --from=publish /app/publish .

# Entry point
ENTRYPOINT ["dotnet", "EmployeeProject.API.dll"]
