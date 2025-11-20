# 🛍️ EShop Microservices

> Полнофункциональная микросервисная архитектура для интернет-магазина с использованием .NET 9.0, Docker и SQL Server

[![.NET](https://img.shields.io/badge/.NET-9.0-purple)](https://dotnet.microsoft.com/)
[![Docker](https://img.shields.io/badge/Docker-Ready-blue)](https://www.docker.com/)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-2019-red)](https://www.microsoft.com/sql-server)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

---

## 📋 Содержание

- [О проекте](#-о-проекте)
- [Архитектура](#-архитектура)
- [Технологический стек](#-технологический-стек)
- [Быстрый старт](#-быстрый-старт)
- [Структура проекта](#-структура-проекта)
- [API Endpoints](#-api-endpoints)
- [Обработка ошибок](#-обработка-ошибок)
- [Конфигурация](#-конфигурация)
- [База данных](#-база-данных)
- [Docker](#-docker)
- [Разработка](#-разработка)
- [Тестирование](#-тестирование)
- [Мониторинг и логирование](#-мониторинг-и-логирование)
- [Production](#-production)
- [Устранение неполадок](#-устранение-неполадок)
- [Дорожная карта](#-дорожная-карта)
- [Вклад в проект](#-вклад-в-проект)
- [Лицензия](#-лицензия)

---

## 🎯 О проекте

**EShop Microservices** - это образовательный проект, демонстрирующий современную микросервисную архитектуру с использованием лучших практик разработки на .NET.

### Ключевые особенности:

- ✅ **Микросервисная архитектура** - независимые, слабо связанные сервисы
- ✅ **API Gateway (Aggregator)** - единая точка входа для всех клиентов
- ✅ **Database per Service** - каждый сервис имеет свою БД
- ✅ **Docker контейнеризация** - легкое развертывание и масштабирование
- ✅ **Глобальная обработка ошибок** - централизованная обработка исключений
- ✅ **Dependency Injection** - слабая связанность компонентов
- ✅ **Entity Framework Core** - ORM для работы с БД
- ✅ **Swagger/OpenAPI** - автоматическая документация API
- ✅ **Retry Policy** - устойчивость к временным сбоям БД
- ✅ **Health Checks** - мониторинг состояния сервисов

---

## 🏗️ Архитектура

### Диаграмма компонентов:

```
┌──────────────────────────────────────────────────────────┐
│                    Client Applications                    │
│              (Web, Mobile, Desktop, etc.)                 │
└────────────────────────┬─────────────────────────────────┘
                         │ HTTP/HTTPS
                         ▼
┌──────────────────────────────────────────────────────────┐
│                  API Gateway (Aggregator)                 │
│                       Port: 5000                          │
│  ┌────────────────────────────────────────────────────┐  │
│  │  - Request Routing                                 │  │
│  │  - Response Aggregation                            │  │
│  │  - Global Exception Handling                       │  │
│  │  - Service Discovery                               │  │
│  └────────────────────────────────────────────────────┘  │
└──────────────┬───────────────┬─────────────┬─────────────┘
               │               │             │
       ┌───────▼──────┐ ┌─────▼──────┐ ┌───▼──────────┐
       │  Customer    │ │   Order    │ │   Product    │
       │  Service     │ │  Service   │ │   Service    │
       │  Port: 5001  │ │ Port: 5002 │ │ Port: 5003   │
       └──────┬───────┘ └─────┬──────┘ └──────┬───────┘
              │               │               │
              ▼               ▼               ▼
       ┌─────────────┐ ┌─────────────┐ ┌─────────────┐
       │  Customer   │ │   Order     │ │  Product    │
       │     DB      │ │     DB      │ │     DB      │
       │ SQL Server  │ │ SQL Server  │ │ SQL Server  │
       │ Port: 1433  │ │ Port: 1434  │ │ Port: 1435  │
       └─────────────┘ └─────────────┘ └─────────────┘
```

### Принципы архитектуры:

1. **Service Independence** - каждый сервис может разрабатываться, развертываться и масштабироваться независимо
2. **Database per Service** - изоляция данных, каждый сервис владеет своей БД
3. **API Gateway Pattern** - единая точка входа, упрощение клиентского кода
4. **Containerization** - упаковка в Docker для consistent deployment
5. **Resilience** - retry policies, circuit breakers, graceful degradation

---

## 🔧 Технологический стек

### Backend:
- **Framework**: .NET 9.0
- **Web**: ASP.NET Core Web API
- **ORM**: Entity Framework Core 9.0
- **Database**: SQL Server 2019
- **API Documentation**: Swagger/OpenAPI (Swashbuckle)
- **API Gateway**: Custom Aggregator with Ocelot

### DevOps:
- **Containerization**: Docker
- **Orchestration**: Docker Compose
- **CI/CD**: (Ready for GitHub Actions, Azure DevOps)

### Patterns & Practices:
- Microservices Architecture
- API Gateway Pattern
- Database per Service Pattern
- Repository Pattern (implicit via EF Core)
- Dependency Injection
- Global Exception Handling
- Retry Pattern
- Health Checks

---

## 🚀 Быстрый старт

### Предварительные требования:

- **Docker Desktop** 4.0+ ([Скачать](https://www.docker.com/products/docker-desktop))
- **Windows 10/11** Pro/Enterprise/Education (для WSL 2)
- **Минимум 8 GB RAM** (рекомендуется 16 GB)
- **20 GB** свободного места на диске
- **Порты свободны**: 5000-5003, 1433-1435

### Установка и запуск:

#### Способ 1: Автоматический (рекомендуется)

```bash
# Windows
start-services.bat
```

Скрипт автоматически:
1. ✅ Запустит все контейнеры
2. ✅ Дождется готовности SQL Server
3. ✅ Создаст базы данных
4. ✅ Перезапустит микросервисы
5. ✅ Покажет статус и URL'ы

#### Способ 2: Ручной

```bash
# 1. Запустить контейнеры
docker-compose up -d

# 2. Дождаться запуска SQL Server (45 секунд)
timeout /t 45

# 3. Создать базы данных
docker exec customer-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "YourStrong@Passw0rd" -C -Q "IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'CustomerDB') CREATE DATABASE [CustomerDB]"
docker exec order-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "YourStrong@Passw0rd" -C -Q "IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'OrderDB') CREATE DATABASE [OrderDB]"
docker exec product-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "YourStrong@Passw0rd" -C -Q "IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'ProductDB') CREATE DATABASE [ProductDB]"

# 4. Перезапустить микросервисы
docker restart customer-service order-service product-service aggregator

# 5. Проверить статус
docker-compose ps
```

#### Способ 3: PowerShell скрипт

```powershell
powershell -ExecutionPolicy Bypass -File .\create-databases.ps1
```

### Проверка работы:

Откройте в браузере:

| Сервис | URL | Описание |
|--------|-----|----------|
| 🌐 **Aggregator** | http://localhost:5000/swagger | **Главная точка входа** (используйте этот!) |
| 👤 Customer | http://localhost:5001/swagger | Управление клиентами |
| 📦 Order | http://localhost:5002/swagger | Управление заказами |
| 🛍️ Product | http://localhost:5003/swagger | Управление товарами |

---

## 📁 Структура проекта

```
EShopMicroservices/
│
├── 📂 Aggregator.WebApi/              # API Gateway
│   ├── Controllers/                   # REST контроллеры
│   ├── Customer/                      # Customer service client
│   ├── Order/                         # Order service client
│   ├── Product/                       # Product service client
│   ├── Middleware/                    # Exception handling
│   ├── Dockerfile                     # Docker образ
│   └── appsettings.json              # Конфигурация
│
├── 📂 Customer.Microservice/          # Микросервис клиентов
│   ├── Controllers/                   # CustomerController
│   ├── Data/                         # DbContext, Repositories
│   ├── Entities/                     # Customer entity
│   ├── Migrations/                   # EF Core migrations
│   ├── Middleware/                   # Exception handling
│   ├── Dockerfile                    # Docker образ
│   └── appsettings.json             # Конфигурация + Connection String
│
├── 📂 Order.Microservice/             # Микросервис заказов
│   ├── Controllers/                   # OrderController
│   ├── Data/                         # DbContext
│   ├── Entities/                     # Order entity
│   ├── Enums/                        # OrderStatus
│   ├── Migrations/                   # EF Core migrations
│   ├── Middleware/                   # Exception handling
│   ├── Dockerfile                    # Docker образ
│   └── appsettings.json             # Конфигурация + Connection String
│
├── 📂 Product.Microservice/           # Микросервис товаров
│   ├── Controllers/                   # ProductController
│   ├── Data/                         # DbContext
│   ├── Entities/                     # Product entity
│   ├── Migrations/                   # EF Core migrations
│   ├── Middleware/                   # Exception handling
│   ├── Dockerfile                    # Docker образ
│   └── appsettings.json             # Конфигурация + Connection String
│
├── 📂 sql-init/                       # SQL инициализационные скрипты
│   ├── init-customer-db.sql
│   ├── init-order-db.sql
│   ├── init-product-db.sql
│   ├── entrypoint-customer.sh
│   ├── entrypoint-order.sh
│   └── entrypoint-product.sh
│
├── 📄 docker-compose.yml              # Оркестрация всех сервисов
├── 📄 .dockerignore                   # Docker игнорируемые файлы
├── 📄 start-services.bat              # Скрипт запуска (Windows)
├── 📄 stop-services.bat               # Скрипт остановки (Windows)
├── 📄 create-databases.ps1            # PowerShell скрипт создания БД
├── 📄 check-docker.ps1                # Проверка готовности Docker
├── 📄 docker-manager.ps1              # Управление контейнерами
│
├── 📚 README.md                       # Этот файл
├── 📚 START-HERE.md                   # Quick start guide
├── 📚 DOCKER-README.md                # Docker документация
├── 📚 AGGREGATOR-REFACTORING.md       # Детали рефакторинга
├── 📚 TROUBLESHOOTING.md              # Решение проблем
└── 📚 CHANGES.md                      # Changelog
```

---

## 🔌 API Endpoints

### Aggregator API (Port 5000)

#### Shop Controller

```http
GET /api/v1/Shop/Customers/{customerName}
```

**Описание**: Получить все заказы и товары клиента по имени  
**Параметры**: 
- `customerName` (string) - Имя клиента

**Пример запроса**:
```bash
curl http://localhost:5000/api/v1/Shop/Customers/Guest
```

**Пример ответа**:
```json
[
  {
    "id": 1,
    "name": "Computer 1",
    "rate": 1000
  },
  {
    "id": 2,
    "name": "Computer 2",
    "rate": 1500
  }
]
```

### Customer Service (Port 5001)

#### Get All Customers
```http
GET /api/v1/Customer
```

#### Get Customer by ID
```http
GET /api/v1/Customer/{id}
```

#### Get Customer by Nickname
```http
GET /api/v1/Customer/Nickname/{nickname}
```

#### Create Customer
```http
POST /api/v1/Customer
Content-Type: application/json

{
  "name": "John Doe",
  "city": "Moscow"
}
```

#### Update Customer
```http
PUT /api/v1/Customer
Content-Type: application/json

{
  "id": 1,
  "name": "John Updated",
  "city": "Saint Petersburg"
}
```

#### Delete Customer
```http
DELETE /api/v1/Customer/{id}
```

### Order Service (Port 5002)

#### Get All Orders
```http
GET /api/v1/Order
```

#### Get Order by ID
```http
GET /api/v1/Order/{id}
```

#### Get Orders by Customer ID
```http
GET /api/v1/Order/Customers/{customerId}
```

#### Create Order
```http
POST /api/v1/Order
Content-Type: application/json

{
  "customerId": 1,
  "productId": 2,
  "orderStatus": 0
}
```

#### Update Order
```http
PUT /api/v1/Order
Content-Type: application/json

{
  "id": 1,
  "customerId": 1,
  "productId": 2,
  "orderStatus": 1
}
```

#### Delete Order
```http
DELETE /api/v1/Order/{id}
```

**OrderStatus enum**:
- `0` - InProgress
- `1` - Completed
- `2` - Cancelled

### Product Service (Port 5003)

#### Get All Products
```http
GET /api/v1/Product
```

#### Get Product by ID
```http
GET /api/v1/Product/{id}
```

#### Create Product
```http
POST /api/v1/Product
Content-Type: application/json

{
  "name": "New Computer",
  "rate": 2500
}
```

#### Update Product
```http
PUT /api/v1/Product
Content-Type: application/json

{
  "id": 1,
  "name": "Updated Computer",
  "rate": 2000
}
```

#### Delete Product
```http
DELETE /api/v1/Product/{id}
```

---

## ⚠️ Обработка ошибок

### Глобальный обработчик исключений

Все микросервисы и Aggregator имеют **глобальный middleware для обработки исключений**, который:

1. ✅ **Перехватывает все необработанные исключения**
2. ✅ **Логирует ошибки** с полной информацией
3. ✅ **Возвращает единообразный JSON ответ**
4. ✅ **Скрывает внутренние детали** в production

### Формат ответа при ошибке:

```json
{
  "statusCode": 500,
  "message": "An error occurred while processing your request.",
  "detailed": "Cannot connect to database server.",
  "timestamp": "2025-11-20T10:30:00.000Z"
}
```

### Типы ошибок:

| Код | Тип | Описание |
|-----|-----|----------|
| 400 | Bad Request | Невалидные данные запроса |
| 404 | Not Found | Ресурс не найден |
| 500 | Internal Server Error | Внутренняя ошибка сервера |
| 503 | Service Unavailable | Микросервис недоступен (только Aggregator) |

### Примеры обработки:

#### В микросервисах:
```csharp
// Customer.Microservice/Middleware/GlobalExceptionHandler.cs
public async Task InvokeAsync(HttpContext context)
{
    try
    {
        await _next(context);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "An unhandled exception occurred");
        await HandleExceptionAsync(context, ex);
    }
}
```

#### В Aggregator:
```csharp
// Aggregator.WebApi/Middleware/GlobalExceptionHandler.cs
catch (HttpRequestException ex)
{
    // Специальная обработка для ошибок связи с микросервисами
    _logger.LogError(ex, "Microservice communication error");
    await HandleExceptionAsync(context, ex, 
        HttpStatusCode.ServiceUnavailable, 
        "Unable to communicate with downstream service.");
}
```

---

## ⚙️ Конфигурация

### Environment Variables

Все сервисы настраиваются через переменные окружения в `docker-compose.yml`:

```yaml
environment:
  - ASPNETCORE_ENVIRONMENT=Development
  - ASPNETCORE_URLS=http://+:80
  - ConnectionStrings__DefaultConnection=Server=...
  - MicroserviceUrls__CustomerService=http://customer-service
```

### appsettings.json

Каждый сервис имеет свой конфигурационный файл:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=customer-db;Database=CustomerDB;..."
  }
}
```

### Docker Compose конфигурация

**Полная конфигурация**: см. `docker-compose.yml`

**Ключевые параметры**:
- Networks: `eshop-network` (bridge)
- Volumes: persistentdata storage для каждой БД
- Restart policy: `on-failure`
- Health checks: для SQL Server контейнеров

---

## 💾 База данных

### Схема данных:

#### CustomerDB
```sql
Table: Customers
├── Id (int, PK, Identity)
├── Name (nvarchar(max))
└── City (nvarchar(max))
```

#### OrderDB
```sql
Table: Orders
├── Id (int, PK, Identity)
├── CustomerId (int)
├── ProductId (int)
├── Date (datetime2)
└── OrderStatus (int)
```

#### ProductDB
```sql
Table: Products
├── Id (int, PK, Identity)
├── Name (nvarchar(max))
└── Rate (decimal(18,2))
```

### Connection Strings:

**Development (локально)**:
```
Server=localhost,{port};Database={name};User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True;
```

**Docker**:
```
Server={service-name};Database={name};User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True;
```

### Подключение через SSMS:

| База данных | Server | Port | User | Password |
|-------------|--------|------|------|----------|
| CustomerDB | localhost | 1433 | sa | YourStrong@Passw0rd |
| OrderDB | localhost | 1434 | sa | YourStrong@Passw0rd |
| ProductDB | localhost | 1435 | sa | YourStrong@Passw0rd |

### Миграции:

```bash
# Создать миграцию
cd Customer.Microservice
dotnet ef migrations add MigrationName

# Применить миграции
dotnet ef database update
```

---

## 🐳 Docker

### Команды управления:

#### Запуск:
```bash
docker-compose up -d
```

#### Остановка:
```bash
docker-compose stop
```

#### Перезапуск:
```bash
docker-compose restart
```

#### Пересборка:
```bash
docker-compose up -d --build
```

#### Логи:
```bash
# Все сервисы
docker-compose logs -f

# Конкретный сервис
docker logs customer-service -f
```

#### Статус:
```bash
docker-compose ps
```

#### Полная очистка (удалит все данные!):
```bash
docker-compose down -v
```

### Образы Docker:

Все сервисы используют **multi-stage build** для оптимизации размера образа:

1. **Stage 1: Build** - SDK образ для компиляции
2. **Stage 2: Publish** - публикация приложения
3. **Stage 3: Runtime** - минимальный runtime образ

---

## 👨‍💻 Разработка

### Локальная разработка (без Docker):

1. **Установите SQL Server** локально
2. **Создайте базы данных**:
```sql
CREATE DATABASE CustomerDB;
CREATE DATABASE OrderDB;
CREATE DATABASE ProductDB;
```

3. **Обновите connection strings** в `appsettings.Development.json`

4. **Запустите сервисы**:
```bash
# Терминал 1
cd Customer.Microservice
dotnet run --urls "http://localhost:5001"

# Терминал 2
cd Order.Microservice
dotnet run --urls "http://localhost:5002"

# Терминал 3
cd Product.Microservice
dotnet run --urls "http://localhost:5003"

# Терминал 4
cd Aggregator.WebApi
dotnet run --urls "http://localhost:5000"
```

### Hot Reload:

```bash
dotnet watch run
```

### Добавление нового микросервиса:

1. Создайте новый ASP.NET Core Web API проект
2. Добавьте DbContext и entities
3. Создайте Dockerfile
4. Добавьте сервис в `docker-compose.yml`
5. Обновите Aggregator для интеграции

---

## 🧪 Тестирование

### Ручное тестирование через Swagger:

1. Откройте http://localhost:5000/swagger
2. Выберите endpoint
3. Нажмите "Try it out"
4. Введите параметры
5. Execute

### Тестирование через curl:

```bash
# Создать клиента
curl -X POST http://localhost:5001/api/v1/Customer \
  -H "Content-Type: application/json" \
  -d '{"name":"Test User","city":"Moscow"}'

# Получить всех клиентов
curl http://localhost:5001/api/v1/Customer

# Через Aggregator
curl http://localhost:5000/api/v1/Shop/Customers/Guest
```

### Postman Collection:

Импортируйте Swagger JSON в Postman:
```
http://localhost:5000/swagger/v1/swagger.json
```

---

## 📊 Мониторинг и логирование

### Логи:

**Просмотр логов в реальном времени**:
```bash
docker logs -f customer-service
```

**Все логи**:
```bash
docker-compose logs
```

### Health Checks:

SQL Server контейнеры имеют встроенные health checks.

**Проверка здоровья**:
```bash
docker inspect customer-sqlserver | grep Health
```

### Метрики:

**Использование ресурсов**:
```bash
docker stats
```

---

## 🚀 Production

### ⚠️ ВАЖНО для Production:

#### 1. Безопасность:

```bash
# Используйте сильные пароли
SA_PASSWORD=<генерируйте случайный 32-символьный пароль>

# Используйте Docker Secrets
docker secret create db_password password.txt
```

#### 2. HTTPS:

Настройте SSL сертификаты для всех сервисов:
```yaml
environment:
  - ASPNETCORE_URLS=https://+:443;http://+:80
  - ASPNETCORE_Kestrel__Certificates__Default__Path=/app/cert.pfx
  - ASPNETCORE_Kestrel__Certificates__Default__Password=<password>
```

#### 3. Логирование:

Интегрируйте с ELK Stack или Azure Application Insights:
```csharp
services.AddApplicationInsightsTelemetry(Configuration["ApplicationInsights:InstrumentationKey"]);
```

#### 4. Масштабирование:

Используйте Kubernetes или Docker Swarm:
```bash
docker stack deploy -c docker-compose.yml eshop
```

#### 5. База данных:

- Используйте managed БД (Azure SQL, AWS RDS)
- Настройте регулярные бэкапы
- Используйте read replicas для масштабирования чтения

#### 6. API Gateway:

Рассмотрите использование:
- Azure API Management
- Kong
- Traefik

---

## 🔧 Устранение неполадок

### Частые проблемы и решения:

#### 1. Docker контейнеры не запускаются

**Проблема**: `Cannot connect to Docker daemon`

**Решение**:
```bash
# Проверьте что Docker Desktop запущен
docker version

# Перезапустите Docker Desktop
```

#### 2. Порты заняты

**Проблема**: `Port 5001 is already allocated`

**Решение**:
```powershell
# Найдите процесс
netstat -ano | findstr :5001

# Убейте процесс
taskkill /PID <PID> /F
```

#### 3. Ошибка подключения к БД

**Проблема**: `Cannot open database "CustomerDB"`

**Решение**:
```bash
# Создайте БД вручную
powershell -ExecutionPolicy Bypass -File .\create-databases.ps1

# Перезапустите сервисы
docker restart customer-service order-service product-service
```

#### 4. Недостаточно памяти

**Проблема**: SQL Server контейнеры падают

**Решение**:
- Docker Desktop → Settings → Resources
- Установите Memory: минимум 8 GB
- Apply & Restart

#### 5. Aggregator не может подключиться к сервисам

**Проблема**: `Service Unavailable (503)`

**Решение**:
```bash
# Проверьте что все сервисы запущены
docker-compose ps

# Проверьте сеть
docker network inspect eshopmicroservices_eshop-network

# Перезапустите aggregator
docker restart aggregator
```

**Подробнее**: см. [TROUBLESHOOTING.md](TROUBLESHOOTING.md)

---

## 🗺️ Дорожная карта

### Планируемые улучшения:

- [ ] **Authentication & Authorization** (JWT, OAuth2)
- [ ] **Message Bus** (RabbitMQ/Kafka для async communication)
- [ ] **Event Sourcing & CQRS**
- [ ] **Service Discovery** (Consul)
- [ ] **Circuit Breaker** (Polly)
- [ ] **Distributed Tracing** (Jaeger, Zipkin)
- [ ] **Centralized Logging** (ELK Stack, Seq)
- [ ] **Monitoring & Alerting** (Prometheus + Grafana)
- [ ] **Unit & Integration Tests**
- [ ] **CI/CD Pipeline** (GitHub Actions)
- [ ] **Kubernetes Deployment**
- [ ] **API Versioning**
- [ ] **Rate Limiting**
- [ ] **Caching** (Redis)
- [ ] **gRPC Communication**
- [ ] **GraphQL Gateway**

---

## 🤝 Вклад в проект

Проект открыт для contributions! Вы можете:

1. 🐛 Сообщить об ошибке (создайте Issue)
2. 💡 Предложить улучшение (создайте Issue)
3. 🔧 Исправить баг (создайте Pull Request)
4. ✨ Добавить новую фичу (создайте Pull Request)

### Как внести вклад:

```bash
# 1. Fork репозиторий
# 2. Создайте ветку
git checkout -b feature/amazing-feature

# 3. Сделайте изменения и commit
git commit -m 'Add amazing feature'

# 4. Push в ветку
git push origin feature/amazing-feature

# 5. Создайте Pull Request
```

---

## 📄 Лицензия

Этот проект распространяется под лицензией MIT. См. файл [LICENSE](LICENSE) для подробностей.

---

## 📞 Контакты и поддержка

- 📧 **Email**: [your-email@example.com](mailto:your-email@example.com)
- 🐛 **Issues**: [GitHub Issues](https://github.com/yourusername/eshop-microservices/issues)
- 📖 **Wiki**: [Project Wiki](https://github.com/yourusername/eshop-microservices/wiki)
- 💬 **Discussions**: [GitHub Discussions](https://github.com/yourusername/eshop-microservices/discussions)

---

## 🙏 Acknowledgments

- Inspired by [eShopOnContainers](https://github.com/dotnet-architecture/eShopOnContainers)
- Built with ❤️ using .NET and Docker
- Special thanks to the .NET community

---

## 📚 Дополнительные ресурсы

### Документация проекта:
- [START-HERE.md](START-HERE.md) - Quick Start Guide
- [DOCKER-README.md](DOCKER-README.md) - Docker Setup
- [AGGREGATOR-REFACTORING.md](AGGREGATOR-REFACTORING.md) - Aggregator Refactoring Details
- [TROUBLESHOOTING.md](TROUBLESHOOTING.md) - Troubleshooting Guide
- [CHANGES.md](CHANGES.md) - Changelog

### Внешние ресурсы:
- [.NET Documentation](https://docs.microsoft.com/dotnet/)
- [Docker Documentation](https://docs.docker.com/)
- [SQL Server Documentation](https://docs.microsoft.com/sql/)
- [Microservices Architecture](https://microservices.io/)
- [The Twelve-Factor App](https://12factor.net/)

---

<div align="center">

**⭐ Поставьте звезду, если проект был полезен! ⭐**

Made with ❤️ for learning and education

[⬆ Вернуться к началу](#️-eshop-microservices)

</div>
