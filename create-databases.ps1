# Script to create databases in SQL Server containers

Write-Host "`nCreating databases for microservices..." -ForegroundColor Cyan

Write-Host "`nWaiting for SQL Server to start (45 seconds)..." -ForegroundColor Yellow
Start-Sleep -Seconds 45

# Function to create database
function Create-Database {
    param(
        [string]$ContainerName,
        [string]$DatabaseName
    )
    
    Write-Host "`nCreating database $DatabaseName in container $ContainerName..." -ForegroundColor Cyan
    
    $createDbCommand = "IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'$DatabaseName') CREATE DATABASE [$DatabaseName]"
    
    for ($i = 1; $i -le 10; $i++) {
        try {
            $result = docker exec $ContainerName /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "YourStrong@Passw0rd" -C -Q $createDbCommand 2>&1
            
            if ($LASTEXITCODE -eq 0) {
                Write-Host "Success! Database $DatabaseName created!" -ForegroundColor Green
                return $true
            } else {
                Write-Host "Attempt $i/10: Waiting for SQL Server..." -ForegroundColor Yellow
                Start-Sleep -Seconds 3
            }
        } catch {
            Write-Host "Attempt $i/10: $($_.Exception.Message)" -ForegroundColor Yellow
            Start-Sleep -Seconds 3
        }
    }
    
    Write-Host "Failed to create database $DatabaseName" -ForegroundColor Red
    return $false
}

# Create databases
$success = $true

$success = $success -and (Create-Database -ContainerName "customer-sqlserver" -DatabaseName "CustomerDB")
$success = $success -and (Create-Database -ContainerName "order-sqlserver" -DatabaseName "OrderDB")
$success = $success -and (Create-Database -ContainerName "product-sqlserver" -DatabaseName "ProductDB")

if ($success) {
    Write-Host "`nAll databases created successfully!" -ForegroundColor Green
    Write-Host "`nRestarting microservices..." -ForegroundColor Cyan
    
    docker restart customer-service order-service product-service | Out-Null
    
    Write-Host "`nWaiting for services to start (15 seconds)..." -ForegroundColor Yellow
    Start-Sleep -Seconds 15
    
    Write-Host "`nContainer status:" -ForegroundColor Cyan
    docker-compose ps
    
    Write-Host "`nDone! Services are available at:" -ForegroundColor Green
    Write-Host "  Customer Service: http://localhost:5001/swagger" -ForegroundColor White
    Write-Host "  Order Service:    http://localhost:5002/swagger" -ForegroundColor White
    Write-Host "  Product Service:  http://localhost:5003/swagger" -ForegroundColor White
} else {
    Write-Host "`nError creating databases!" -ForegroundColor Red
}
