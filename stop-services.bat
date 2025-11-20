@echo off
echo ========================================
echo  EShop Microservices - Stopping...
echo ========================================
echo.

docker-compose stop

if %errorlevel% equ 0 (
    echo.
    echo ========================================
    echo  Services stopped successfully!
    echo ========================================
    echo.
) else (
    echo.
    echo ========================================
    echo  Error stopping services!
    echo ========================================
    echo.
)

pause

