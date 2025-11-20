# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["Customer.Microservice/Customer.Microservice.csproj", "Customer.Microservice/"]
RUN dotnet restore "Customer.Microservice/Customer.Microservice.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/Customer.Microservice"
RUN dotnet build "Customer.Microservice.csproj" -c Release -o /app/build

# Stage 2: Publish
FROM build AS publish
RUN dotnet publish "Customer.Microservice.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 3: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Customer.Microservice.dll"]

