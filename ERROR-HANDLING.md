# 🛡️ Обработка ошибок в EShop Microservices

## Обзор

Все микросервисы и Aggregator имеют **глобальную централизованную обработку исключений** через custom middleware.

---

## 📋 Архитектура обработки ошибок

```
┌────────────────────────────────────────┐
│         HTTP Request                    │
└──────────────┬─────────────────────────┘
               │
               ▼
┌──────────────────────────────────────────┐
│   Global Exception Handler Middleware    │
│   ┌────────────────────────────────────┐ │
│   │  try {                             │ │
│   │    await _next(context);           │ │
│   │  } catch (Exception ex) {          │ │
│   │    Log error                       │ │
│   │    Return formatted JSON response  │ │
│   │  }                                 │ │
│   └────────────────────────────────────┘ │
└──────────────┬───────────────────────────┘
               │
               ▼
┌──────────────────────────────────────────┐
│       Application Pipeline               │
│   (Controllers, Services, Database)      │
└──────────────────────────────────────────┘
```

---

## 🔧 Реализация

### 1. Middleware для микросервисов

**Путь**: `{Service}.Microservice/Middleware/GlobalExceptionHandler.cs`

```csharp
public class GlobalExceptionHandler
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(
        RequestDelegate next, 
        ILogger<GlobalExceptionHandler> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(
        HttpContext context, 
        Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var response = new
        {
            statusCode = context.Response.StatusCode,
            message = "An error occurred while processing your request.",
            detailed = exception.Message,
            timestamp = DateTime.UtcNow
        };

        return context.Response.WriteAsync(
            JsonSerializer.Serialize(response));
    }
}
```

### 2. Middleware для Aggregator

**Путь**: `Aggregator.WebApi/Middleware/GlobalExceptionHandler.cs`

**Особенность**: Специальная обработка для ошибок связи с микросервисами.

```csharp
public async Task InvokeAsync(HttpContext context)
{
    try
    {
        await _next(context);
    }
    catch (HttpRequestException ex)
    {
        // Ошибка связи с микросервисом
        _logger.LogError(ex, "Microservice communication error");
        await HandleExceptionAsync(context, ex, 
            HttpStatusCode.ServiceUnavailable, 
            "Unable to communicate with downstream service.");
    }
    catch (Exception ex)
    {
        // Любая другая ошибка
        _logger.LogError(ex, "Unhandled exception");
        await HandleExceptionAsync(context, ex, 
            HttpStatusCode.InternalServerError, 
            "An error occurred while processing your request.");
    }
}
```

### 3. Регистрация в Startup.cs

```csharp
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    // ВАЖНО: Middleware должен быть первым!
    app.UseMiddleware<Middleware.GlobalExceptionHandler>();
    
    if (env.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }
    
    // ... остальная конфигурация
}
```

---

## 📊 Формат ответа при ошибке

### Стандартный ответ:

```json
{
  "statusCode": 500,
  "message": "An error occurred while processing your request.",
  "detailed": "Object reference not set to an instance of an object.",
  "timestamp": "2025-11-20T12:30:00.000Z"
}
```

### Ответ при ошибке связи (только Aggregator):

```json
{
  "statusCode": 503,
  "message": "Unable to communicate with downstream service.",
  "detailed": "No connection could be made because the target machine actively refused it.",
  "timestamp": "2025-11-20T12:30:00.000Z"
}
```

---

## 🔍 Типы ошибок

### HTTP Status Codes

| Код | Название | Когда возвращается | Пример |
|-----|----------|-------------------|---------|
| 400 | Bad Request | Невалидные данные | Missing required field |
| 404 | Not Found | Ресурс не найден | Customer not found |
| 500 | Internal Server Error | Ошибка сервера | Database connection failed |
| 503 | Service Unavailable | Сервис недоступен | Microservice is down |

### Типы исключений

#### В микросервисах:

1. **DbUpdateException** - ошибки при работе с БД
   ```
   Cannot insert explicit value for identity column
   ```

2. **SqlException** - ошибки SQL Server
   ```
   Cannot open database requested by the login
   ```

3. **ArgumentException** - невалидные аргументы
   ```
   Value cannot be null. Parameter name: customerName
   ```

#### В Aggregator:

1. **HttpRequestException** - ошибки связи с микросервисами
   ```
   No connection could be made
   ```

2. **TaskCanceledException** - таймаут запроса
   ```
   A task was canceled
   ```

---

## 📝 Примеры использования

### 1. Ошибка в Customer Service

**Запрос**:
```http
GET /api/v1/Customer/999
```

**Ответ (404)**:
```json
{
  "statusCode": 404,
  "message": "Customer not found",
  "detailed": "Customer with ID 999 does not exist",
  "timestamp": "2025-11-20T12:30:00.000Z"
}
```

### 2. Ошибка базы данных

**Запрос**:
```http
POST /api/v1/Customer
Content-Type: application/json

{
  "name": null,
  "city": "Moscow"
}
```

**Ответ (500)**:
```json
{
  "statusCode": 500,
  "message": "An error occurred while processing your request.",
  "detailed": "Cannot insert the value NULL into column 'Name'",
  "timestamp": "2025-11-20T12:30:00.000Z"
}
```

### 3. Микросервис недоступен (через Aggregator)

**Запрос**:
```http
GET /api/v1/Shop/Customers/Guest
```

**Сценарий**: Customer Service не запущен

**Ответ (503)**:
```json
{
  "statusCode": 503,
  "message": "Unable to communicate with downstream service.",
  "detailed": "No connection could be made to http://customer-service",
  "timestamp": "2025-11-20T12:30:00.000Z"
}
```

---

## 🔐 Безопасность

### Production режим

В production **НЕ показывайте** детали ошибок пользователям:

```csharp
private static Task HandleExceptionAsync(
    HttpContext context, 
    Exception exception)
{
    var isDevelopment = context.RequestServices
        .GetRequiredService<IWebHostEnvironment>()
        .IsDevelopment();
    
    var response = new
    {
        statusCode = context.Response.StatusCode,
        message = "An error occurred while processing your request.",
        detailed = isDevelopment ? exception.Message : null,
        timestamp = DateTime.UtcNow
    };
    
    return context.Response.WriteAsync(
        JsonSerializer.Serialize(response));
}
```

### Что НЕ показывать:

- ❌ Stack traces
- ❌ Пути к файлам
- ❌ Connection strings
- ❌ Имена таблиц БД
- ❌ Внутренние IP адреса

### Что показывать:

- ✅ Общее сообщение об ошибке
- ✅ HTTP status code
- ✅ Timestamp
- ✅ Request ID (для трейсинга)

---

## 📊 Логирование

### Уровни логирования

```csharp
// Critical - система неработоспособна
_logger.LogCritical(ex, "Database server is down");

// Error - ошибка, но система работает
_logger.LogError(ex, "Failed to process order {OrderId}", orderId);

// Warning - потенциальная проблема
_logger.LogWarning("High latency detected: {Ms}ms", latency);

// Information - важное событие
_logger.LogInformation("Customer {Id} created successfully", customerId);

// Debug - детальная информация для отладки
_logger.LogDebug("Processing request with parameters: {@Params}", params);
```

### Structured Logging

```csharp
_logger.LogError(ex, 
    "Failed to create order. CustomerId: {CustomerId}, ProductId: {ProductId}",
    order.CustomerId, 
    order.ProductId);
```

Это позволяет легко искать и фильтровать логи в системах мониторинга.

---

## 🧪 Тестирование обработки ошибок

### 1. Тест обработки исключения

```csharp
[Fact]
public async Task GlobalExceptionHandler_Should_Return_500_On_Exception()
{
    // Arrange
    var context = new DefaultHttpContext();
    var logger = Mock.Of<ILogger<GlobalExceptionHandler>>();
    
    RequestDelegate next = (HttpContext ctx) => 
        throw new Exception("Test exception");
    
    var middleware = new GlobalExceptionHandler(next, logger);
    
    // Act
    await middleware.InvokeAsync(context);
    
    // Assert
    Assert.Equal(500, context.Response.StatusCode);
}
```

### 2. Тест через API

```bash
# Намеренно вызываем ошибку
curl -X GET http://localhost:5001/api/v1/Customer/99999 -v
```

Проверьте:
- ✅ HTTP status code = 404
- ✅ Content-Type = application/json
- ✅ Тело ответа содержит error details
- ✅ Лог содержит информацию об ошибке

---

## 🚀 Улучшения для Production

### 1. Correlation ID

Добавьте уникальный ID для отслеживания запроса:

```csharp
private static Task HandleExceptionAsync(
    HttpContext context, 
    Exception exception)
{
    var correlationId = Guid.NewGuid().ToString();
    context.Response.Headers.Add("X-Correlation-ID", correlationId);
    
    _logger.LogError(ex, 
        "Error occurred. CorrelationId: {CorrelationId}", 
        correlationId);
    
    var response = new
    {
        statusCode = context.Response.StatusCode,
        message = "An error occurred",
        correlationId = correlationId,
        timestamp = DateTime.UtcNow
    };
    
    return context.Response.WriteAsync(
        JsonSerializer.Serialize(response));
}
```

### 2. Retry Policy (с Polly)

```csharp
services.AddHttpClient<ICustomerService, CustomerService>()
    .AddTransientHttpErrorPolicy(policy => 
        policy.WaitAndRetryAsync(3, 
            retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))));
```

### 3. Circuit Breaker

```csharp
services.AddHttpClient<ICustomerService, CustomerService>()
    .AddTransientHttpErrorPolicy(policy => 
        policy.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));
```

### 4. Health Checks

```csharp
services.AddHealthChecks()
    .AddSqlServer(connectionString)
    .AddUrlGroup(new Uri("http://customer-service/health"), "Customer Service");

app.MapHealthChecks("/health");
```

---

## 📚 Best Practices

### ✅ DO:

1. **Всегда логируйте ошибки** с полным контекстом
2. **Используйте typed exceptions** для разных сценариев
3. **Возвращайте HTTP status codes корректно**
4. **Скрывайте внутренние детали** в production
5. **Добавляйте correlation IDs** для трейсинга
6. **Используйте structured logging**
7. **Тестируйте error scenarios**

### ❌ DON'T:

1. **Не возвращайте stack traces** пользователям
2. **Не игнорируйте исключения** (пустые catch блоки)
3. **Не используйте exceptions для flow control**
4. **Не логируйте sensitive data** (пароли, токены)
5. **Не показывайте SQL queries** в ответах
6. **Не используйте generic exceptions везде**
7. **Не забывайте про async/await** в middleware

---

## 🎯 Итоги

Правильная обработка ошибок:

- ✅ **Улучшает UX** - понятные сообщения об ошибках
- ✅ **Упрощает debugging** - структурированные логи
- ✅ **Повышает безопасность** - скрытие внутренних деталей
- ✅ **Облегчает мониторинг** - централизованная обработка
- ✅ **Повышает надежность** - graceful degradation

---

<div align="center">

**💡 Хорошая обработка ошибок = Хороший пользовательский опыт**

[⬆ Вернуться к главному README](README.md)

</div>

