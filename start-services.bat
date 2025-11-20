@echo off
echo ========================================
echo  EShop Microservices - Starting...
echo ========================================
echo.

echo Step 1: Starting containers...
docker-compose up -d

if %errorlevel% neq 0 (
    echo.
    echo ========================================
    echo  Error starting containers!
    echo ========================================
    pause
    exit /b 1
)

echo.
echo Step 2: Waiting for SQL Server to start (45 seconds)...
timeout /t 45 /nobreak >nul

echo.
echo Step 3: Creating databases...
docker exec customer-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "YourStrong@Passw0rd" -C -Q "IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'CustomerDB') CREATE DATABASE [CustomerDB]"
docker exec order-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "YourStrong@Passw0rd" -C -Q "IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'OrderDB') CREATE DATABASE [OrderDB]"
docker exec product-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "YourStrong@Passw0rd" -C -Q "IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'ProductDB') CREATE DATABASE [ProductDB]"

echo.
echo Step 4: Restarting microservices...
docker restart customer-service order-service product-service >nul

echo.
echo Step 5: Waiting for services to start (15 seconds)...
timeout /t 15 /nobreak >nul

echo.
echo ========================================
echo  All services started successfully!
echo ========================================
echo.
echo  Customer Service: http://localhost:5001/swagger
echo  Order Service:    http://localhost:5002/swagger
echo  Product Service:  http://localhost:5003/swagger
echo.
echo Press any key to view container status...
pause >nul

docker-compose ps

echo.
pause
