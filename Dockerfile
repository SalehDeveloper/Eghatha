# =========================
# Build stage
# =========================
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src

# Copy project files first
COPY ["Eghatha.Api/Eghatha.Api.csproj", "Eghatha.Api/"]
COPY ["Eghatha.Application/Eghatha.Application.csproj", "Eghatha.Application/"]
COPY ["Eghatha.Contract/Eghatha.Contract.csproj", "Eghatha.Contract/"]
COPY ["Eghatha.Domain/Eghatha.Domain.csproj", "Eghatha.Domain/"]
COPY ["Eghatha.Infastructure/Eghatha.Infastructure.csproj", "Eghatha.Infastructure/"]

# Restore dependencies
RUN dotnet restore "Eghatha.Api/Eghatha.Api.csproj"

# Copy the rest of the source code
COPY . .

# Publish the API
WORKDIR "/src/Eghatha.Api"

RUN dotnet publish "Eghatha.Api.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore


# =========================
# Runtime stage
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final

WORKDIR /app

ENV ASPNETCORE_HTTP_PORTS=8080

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "Eghatha.Api.dll"]