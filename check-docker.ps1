# Скрипт проверки Docker
Write-Host "`n🔍 Проверка Docker..." -ForegroundColor Cyan

# Проверка запущен ли Docker Desktop
$dockerProcess = Get-Process "Docker Desktop" -ErrorAction SilentlyContinue

if (-not $dockerProcess) {
    Write-Host "❌ Docker Desktop не запущен!" -ForegroundColor Red
    Write-Host "Запускаем Docker Desktop..." -ForegroundColor Yellow
    Start-Process "C:\Program Files\Docker\Docker\Docker Desktop.exe"
    Write-Host "⏳ Ожидайте 30 секунд, пока Docker полностью запустится..." -ForegroundColor Yellow
    Start-Sleep -Seconds 30
}

# Ожидание готовности Docker
Write-Host "`n⏳ Ожидание готовности Docker..." -ForegroundColor Yellow

$maxAttempts = 30
$attempt = 0
$dockerReady = $false

while ($attempt -lt $maxAttempts) {
    $null = docker info 2>&1
    if ($LASTEXITCODE -eq 0) {
        $dockerReady = $true
        break
    }
    
    Write-Host "." -NoNewline -ForegroundColor Gray
    Start-Sleep -Seconds 2
    $attempt++
}

if (-not $dockerReady) {
    Write-Host "`n❌ Timeout: Docker не запустился за отведенное время" -ForegroundColor Red
    Write-Host "Попробуйте:" -ForegroundColor Yellow
    Write-Host "  1. Перезапустить Docker Desktop вручную" -ForegroundColor White
    Write-Host "  2. Проверить логи Docker Desktop" -ForegroundColor White
    Write-Host "  3. Перезагрузить компьютер" -ForegroundColor White
    exit 1
}

Write-Host "`n✅ Docker готов к работе!" -ForegroundColor Green

# Информация о Docker
Write-Host "`n📊 Информация о Docker:" -ForegroundColor Cyan
docker --version
docker-compose --version

# Проверка занятых портов
Write-Host "`n🔌 Проверка портов:" -ForegroundColor Cyan
$ports = @(5001, 5002, 5003, 1433, 1434, 1435)
$portsInUse = @()

foreach ($port in $ports) {
    $connection = Get-NetTCPConnection -LocalPort $port -ErrorAction SilentlyContinue
    if ($connection) {
        $portsInUse += $port
        Write-Host "  ⚠️  Порт $port занят" -ForegroundColor Yellow
    } else {
        Write-Host "  ✅ Порт $port свободен" -ForegroundColor Green
    }
}

if ($portsInUse.Count -gt 0) {
    Write-Host "`n⚠️  Некоторые порты заняты. Возможно потребуется остановить процессы." -ForegroundColor Yellow
}

# Проверка доступности образа SQL Server
Write-Host "`n🐋 Загрузка образа SQL Server 2019..." -ForegroundColor Cyan
Write-Host "Это может занять несколько минут при первом запуске..." -ForegroundColor Gray

docker pull mcr.microsoft.com/mssql/server:2019-latest

if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ Образ SQL Server 2019 успешно загружен!" -ForegroundColor Green
} else {
    Write-Host "❌ Ошибка загрузки образа SQL Server!" -ForegroundColor Red
    Write-Host "`n💡 Возможные решения:" -ForegroundColor Yellow
    Write-Host "  1. Проверьте подключение к интернету" -ForegroundColor White
    Write-Host "  2. Проверьте настройки proxy/firewall" -ForegroundColor White
    Write-Host "  3. В Docker Desktop: Settings → Docker Engine → добавьте registry-mirrors" -ForegroundColor White
    Write-Host "  4. Смотрите TROUBLESHOOTING.md для подробных инструкций" -ForegroundColor White
    exit 1
}

Write-Host "`n✨ Все проверки завершены!" -ForegroundColor Green
Write-Host "Теперь можете запустить сервисы командой:" -ForegroundColor Cyan
Write-Host "  docker-compose up -d" -ForegroundColor White
Write-Host "или используйте:" -ForegroundColor Cyan
Write-Host "  .\start-services.bat" -ForegroundColor White
