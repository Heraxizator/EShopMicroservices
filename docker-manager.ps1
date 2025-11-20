# Docker Manager Script для EShop Microservices
# Этот скрипт упрощает управление Docker контейнерами

param(
    [Parameter(Position=0)]
    [ValidateSet('start', 'stop', 'restart', 'rebuild', 'logs', 'status', 'clean', 'help')]
    [string]$Command = 'help'
)

function Show-Help {
    Write-Host "`n=== EShop Microservices Docker Manager ===" -ForegroundColor Cyan
    Write-Host "`nДоступные команды:" -ForegroundColor Yellow
    Write-Host "  start    - Запустить все сервисы" -ForegroundColor Green
    Write-Host "  stop     - Остановить все сервисы" -ForegroundColor Green
    Write-Host "  restart  - Перезапустить все сервисы" -ForegroundColor Green
    Write-Host "  rebuild  - Пересобрать и запустить сервисы" -ForegroundColor Green
    Write-Host "  logs     - Показать логи всех сервисов" -ForegroundColor Green
    Write-Host "  status   - Показать статус всех контейнеров" -ForegroundColor Green
    Write-Host "  clean    - Остановить и удалить все контейнеры и volumes" -ForegroundColor Green
    Write-Host "  help     - Показать эту справку" -ForegroundColor Green
    Write-Host "`nПример использования:" -ForegroundColor Yellow
    Write-Host "  .\docker-manager.ps1 start" -ForegroundColor White
    Write-Host "  .\docker-manager.ps1 logs" -ForegroundColor White
    Write-Host ""
}

function Start-Services {
    Write-Host "`n🚀 Запуск всех сервисов..." -ForegroundColor Cyan
    docker-compose up -d
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host "`n✅ Сервисы успешно запущены!" -ForegroundColor Green
        Write-Host "`nДоступные endpoints:" -ForegroundColor Yellow
        Write-Host "  Customer Service: http://localhost:5001/swagger" -ForegroundColor White
        Write-Host "  Order Service:    http://localhost:5002/swagger" -ForegroundColor White
        Write-Host "  Product Service:  http://localhost:5003/swagger" -ForegroundColor White
        Write-Host "`nДля просмотра логов используйте: .\docker-manager.ps1 logs" -ForegroundColor Gray
    } else {
        Write-Host "`n❌ Ошибка при запуске сервисов!" -ForegroundColor Red
    }
}

function Stop-Services {
    Write-Host "`n⏸️  Остановка всех сервисов..." -ForegroundColor Cyan
    docker-compose stop
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host "`n✅ Сервисы успешно остановлены!" -ForegroundColor Green
    } else {
        Write-Host "`n❌ Ошибка при остановке сервисов!" -ForegroundColor Red
    }
}

function Restart-Services {
    Write-Host "`n🔄 Перезапуск всех сервисов..." -ForegroundColor Cyan
    docker-compose restart
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host "`n✅ Сервисы успешно перезапущены!" -ForegroundColor Green
    } else {
        Write-Host "`n❌ Ошибка при перезапуске сервисов!" -ForegroundColor Red
    }
}

function Rebuild-Services {
    Write-Host "`n🔨 Пересборка и запуск всех сервисов..." -ForegroundColor Cyan
    docker-compose up -d --build
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host "`n✅ Сервисы успешно пересобраны и запущены!" -ForegroundColor Green
    } else {
        Write-Host "`n❌ Ошибка при пересборке сервисов!" -ForegroundColor Red
    }
}

function Show-Logs {
    Write-Host "`n📋 Показ логов всех сервисов (Ctrl+C для выхода)..." -ForegroundColor Cyan
    Write-Host ""
    docker-compose logs -f
}

function Show-Status {
    Write-Host "`n📊 Статус контейнеров:" -ForegroundColor Cyan
    Write-Host ""
    docker-compose ps
    
    Write-Host "`n💾 Использование ресурсов:" -ForegroundColor Cyan
    Write-Host ""
    docker stats --no-stream --format "table {{.Name}}\t{{.CPUPerc}}\t{{.MemUsage}}\t{{.NetIO}}"
}

function Clean-Everything {
    Write-Host "`n⚠️  ВНИМАНИЕ: Эта команда удалит все контейнеры и данные баз данных!" -ForegroundColor Yellow
    $confirmation = Read-Host "Вы уверены? (yes/no)"
    
    if ($confirmation -eq 'yes') {
        Write-Host "`n🧹 Очистка всех контейнеров и volumes..." -ForegroundColor Cyan
        docker-compose down -v
        
        if ($LASTEXITCODE -eq 0) {
            Write-Host "`n✅ Все контейнеры и volumes успешно удалены!" -ForegroundColor Green
        } else {
            Write-Host "`n❌ Ошибка при удалении!" -ForegroundColor Red
        }
    } else {
        Write-Host "`n❌ Отменено пользователем" -ForegroundColor Yellow
    }
}

# Проверка наличия Docker
if (-not (Get-Command docker -ErrorAction SilentlyContinue)) {
    Write-Host "`n❌ Docker не установлен или не найден в PATH!" -ForegroundColor Red
    Write-Host "Пожалуйста, установите Docker Desktop: https://www.docker.com/products/docker-desktop" -ForegroundColor Yellow
    exit 1
}

# Проверка наличия docker-compose
if (-not (Get-Command docker-compose -ErrorAction SilentlyContinue)) {
    Write-Host "`n❌ Docker Compose не установлен или не найден в PATH!" -ForegroundColor Red
    exit 1
}

# Выполнение команды
switch ($Command) {
    'start'   { Start-Services }
    'stop'    { Stop-Services }
    'restart' { Restart-Services }
    'rebuild' { Rebuild-Services }
    'logs'    { Show-Logs }
    'status'  { Show-Status }
    'clean'   { Clean-Everything }
    'help'    { Show-Help }
    default   { Show-Help }
}

