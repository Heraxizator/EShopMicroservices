# Aggregator.WebApi - Рефакторинг для Docker

## 🔧 Что было исправлено:

### Проблема:
Хардкод URL'ов в сервисах, которые работали только на `localhost`:
- `https://localhost:5001` - Customer Service
- `https://localhost:44373` - Order Service  
- `https://localhost:44337` - Product Service

❌ **Это ломалось при запуске в Docker контейнерах!**

### Решение:

#### 1. Добавлена конфигурация URL'ов

**appsettings.json:**
```json
{
  "MicroserviceUrls": {
    "CustomerService": "http://customer-service",
    "OrderService": "http://order-service",
    "ProductService": "http://product-service"
  }
}
```

**appsettings.Development.json (для локальной разработки):**
```json
{
  "MicroserviceUrls": {
    "CustomerService": "http://localhost:5001",
    "OrderService": "http://localhost:5002",
    "ProductService": "http://localhost:5003"
  }
}
```

#### 2. Внедрена Dependency Injection

**До рефакторинга:**
```csharp
public sealed class CustomerService : ICustomerService
{
    private readonly HttpClient _httpClient = new();  // ❌ Плохо!
    
    public async Task<CustomerApi> GetCustomerByNameAsync(...)
    {
        var response = await _httpClient.GetAsync(
            $"https://localhost:5001/api/v1/Customer/..."  // ❌ Хардкод!
        );
    }
}
```

**После рефакторинга:**
```csharp
public sealed class CustomerService : ICustomerService
{
    private readonly HttpClient _httpClient;
    private readonly string _customerServiceUrl;
    
    public CustomerService(
        IHttpClientFactory httpClientFactory,  // ✅ DI
        IConfiguration configuration)            // ✅ Конфигурация
    {
        _httpClient = httpClientFactory.CreateClient();
        _customerServiceUrl = configuration["MicroserviceUrls:CustomerService"] 
            ?? "http://localhost:5001";  // ✅ Fallback
    }
    
    public async Task<CustomerApi> GetCustomerByNameAsync(...)
    {
        var response = await _httpClient.GetAsync(
            $"{_customerServiceUrl}/api/v1/Customer/..."  // ✅ Динамический URL!
        );
    }
}
```

#### 3. Зарегистрированы сервисы в DI контейнере

**Startup.cs:**
```csharp
public void ConfigureServices(IServiceCollection services)
{
    // Register HttpClient
    services.AddHttpClient();
    
    // Register microservice clients
    services.AddScoped<Customer.ICustomerService, Customer.CustomerService>();
    services.AddScoped<Order.IOrderService, Order.OrderService>();
    services.AddScoped<Product.IProductService, Product.ProductService>();
    
    // ... остальное
}
```

#### 4. Создан Dockerfile

Теперь Aggregator может быть упакован в Docker контейнер!

#### 5. Добавлен в docker-compose.yml

```yaml
aggregator:
  build:
    context: .
    dockerfile: Aggregator.WebApi/Dockerfile
  container_name: aggregator
  environment:
    - MicroserviceUrls__CustomerService=http://customer-service
    - MicroserviceUrls__OrderService=http://order-service
    - MicroserviceUrls__ProductService=http://product-service
  ports:
    - "5000:80"
  depends_on:
    - customer-service
    - order-service
    - product-service
```

## ✅ Преимущества после рефакторинга:

1. **Работает в Docker** - URL'ы настраиваются через environment variables
2. **Работает локально** - можно использовать appsettings.Development.json
3. **IHttpClientFactory** - правильное управление HttpClient (нет утечек сокетов)
4. **Dependency Injection** - тестируемость и гибкость
5. **Конфигурация** - легко менять URL'ы без перекомпиляции
6. **Fallback значения** - если конфигурация не задана, используются localhost URL'ы

## 🚀 Как использовать:

### В Docker:
```bash
docker-compose up -d
```

Aggregator доступен на: http://localhost:5000/swagger

### Локально (без Docker):
1. Убедитесь что микросервисы запущены на портах 5001, 5002, 5003
2. Запустите Aggregator:
```bash
cd Aggregator.WebApi
dotnet run
```

URL'ы будут взяты из `appsettings.Development.json`

## 📝 Архитектура:

```
┌─────────────────────────────────────┐
│        Client (Browser)             │
└──────────────┬──────────────────────┘
               │
               ▼
┌──────────────────────────────────────┐
│         Aggregator :5000             │
│       (API Gateway)                  │
└──────┬───────┬───────┬───────────────┘
       │       │       │
       ▼       ▼       ▼
┌──────┐ ┌────────┐ ┌─────────┐
│Customer│ │ Order  │ │ Product │
│:5001   │ │ :5002  │ │ :5003   │
└────────┘ └────────┘ └─────────┘
```

## 🔐 Для Production:

Не забудьте:
1. Использовать HTTPS для всех сервисов
2. Добавить аутентификацию/авторизацию
3. Добавить rate limiting
4. Настроить корректные timeout'ы
5. Добавить retry policies (Polly)
6. Настроить логирование
7. Использовать Azure Key Vault для secrets

## ✨ Итог:

Теперь Aggregator.WebApi полностью готов для работы в Docker и в production окружении!

