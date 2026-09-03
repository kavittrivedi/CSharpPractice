### 1. Multiple implementations of the same interface

**Question:**
You have `INotificationService` with `EmailNotificationService` and `SmsNotificationService`. How would you inject and select the required implementation?

**Answer:**

> “I can register both implementations with DI and inject `IEnumerable<INotificationService>` to get all implementations. Then I can select the required implementation based on a key, type, or business condition. In newer .NET versions, I can also use **Keyed Services**.”

```csharp
builder.Services.AddKeyedScoped<INotificationService, EmailNotificationService>("email");
builder.Services.AddKeyedScoped<INotificationService, SmsNotificationService>("sms");
```

In **.NET 10**, this is much cleaner because we have **Keyed Services** built into the DI container. You can register multiple implementations of the same interface with different keys and inject exactly the one you want. ([Microsoft Learn][1])

### 1. Interface

```csharp
public interface INotificationService
{
    void Send(string message);
}
```

### 2. Implementations

```csharp
public class EmailNotificationService : INotificationService
{
    public void Send(string message)
    {
        Console.WriteLine($"Email: {message}");
    }
}
```

```csharp
public class SmsNotificationService : INotificationService
{
    public void Send(string message)
    {
        Console.WriteLine($"SMS: {message}");
    }
}
```

### 3. Register in `Program.cs`

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddKeyedScoped<INotificationService,
    EmailNotificationService>("email");

builder.Services.AddKeyedScoped<INotificationService,
    SmsNotificationService>("sms");

builder.Services.AddScoped<OrderService>();

var app = builder.Build();

app.MapControllers();

app.Run();
```

Now we have:

```text
"email" → EmailNotificationService
"sms"   → SmsNotificationService
```

### 4. Inject Email into `OrderService`

```csharp
using Microsoft.Extensions.DependencyInjection;

public class OrderService
{
    private readonly INotificationService _notificationService;

    public OrderService(
        [FromKeyedServices("email")]
        INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public void PlaceOrder()
    {
        _notificationService.Send("Order placed successfully");
    }
}
```

The DI container automatically gives you:

```text
OrderService
     ↓
INotificationService
     ↓
"email"
     ↓
EmailNotificationService
```

`[FromKeyedServices]` is specifically provided to resolve a service using its registration key. ([Microsoft Learn][1])

### 5. If you want SMS

Just change the key:

```csharp
public OrderService(
    [FromKeyedServices("sms")]
    INotificationService notificationService)
{
    _notificationService = notificationService;
}
```

Now:

```text
OrderService
     ↓
INotificationService
     ↓
"sms"
     ↓
SmsNotificationService
```

### ⭐ But what if selection is dynamic?

For example:

```text
User chooses Email → email
User chooses SMS   → sms
```

Then don't hard-code `[FromKeyedServices("email")]`.

You can resolve the keyed service dynamically:

```csharp
public class OrderService
{
    private readonly IServiceProvider _serviceProvider;

    public OrderService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void Send(string type, string message)
    {
        var service = _serviceProvider
            .GetRequiredKeyedService<INotificationService>(type);

        service.Send(message);
    }
}
```

Then:

```csharp
orderService.Send("email", "Order placed");
orderService.Send("sms", "Order shipped");
```

### 🎯 Best interview answer

> **"In .NET 6, I would typically use `IEnumerable<INotificationService>` or a factory to select between multiple implementations. In modern .NET, including .NET 10, I can use the built-in Keyed Services feature. I register each implementation with a key using `AddKeyedScoped`, and inject the required implementation using `[FromKeyedServices]`. If the implementation needs to be selected dynamically at runtime, I can use `GetRequiredKeyedService<T>(key)` or encapsulate that logic in a factory."** ([Microsoft Learn][2])

**This is a very good interview topic because it tests both DI and Factory/Strategy patterns.**

[1]: https://learn.microsoft.com/en-us/aspnet/core/mvc/controllers/dependency-injection?view=aspnetcore-10.0&utm_source=chatgpt.com "Dependency injection into controllers in ASP.NET Core | Microsoft Learn"
[2]: https://learn.microsoft.com/sr-cyrl-rs/dotnet/core/extensions/dependency-injection?utm_source=chatgpt.com "Dependency injection - .NET | Microsoft Learn"


Sure. Let's take a **complete .NET 6 example** from registration → implementation → injection → usage.

### 1. Create the interface

```csharp
public interface INotificationService
{
    void Send(string message);
}
```

### 2. Email implementation

```csharp
public class EmailNotificationService : INotificationService
{
    public void Send(string message)
    {
        Console.WriteLine($"Email sent: {message}");
    }
}
```

### 3. SMS implementation

```csharp
public class SmsNotificationService : INotificationService
{
    public void Send(string message)
    {
        Console.WriteLine($"SMS sent: {message}");
    }
}
```

Both classes implement the same interface but have their own implementation.

---

## 4. Register dependencies in .NET 6

In `Program.cs`:

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<INotificationService, EmailNotificationService>();
builder.Services.AddScoped<INotificationService, SmsNotificationService>();

builder.Services.AddScoped<OrderService>();

var app = builder.Build();

app.MapControllers();

app.Run();
```

⚠️ **Important:** If you register both like this and inject a single `INotificationService`, the default .NET DI container will resolve the **last registered implementation**, which is `SmsNotificationService`.

So this:

```csharp
public OrderService(INotificationService notificationService)
```

will get `SmsNotificationService`.

---

# 5. Inject into another class

Suppose we have `OrderService`:

```csharp
public class OrderService
{
    private readonly INotificationService _notificationService;

    public OrderService(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public void PlaceOrder()
    {
        // Order processing...

        _notificationService.Send("Order placed successfully.");
    }
}
```

The DI container automatically creates and injects the registered implementation.

---

# But how do we choose Email OR SMS?

This is the important part of your interview question.

In **.NET 6**, one common solution is to inject **all implementations**:

```csharp
public class OrderService
{
    private readonly IEnumerable<INotificationService> _notificationServices;

    public OrderService(IEnumerable<INotificationService> notificationServices)
    {
        _notificationServices = notificationServices;
    }

    public void SendNotification(string type, string message)
    {
        INotificationService service;

        if (type == "email")
        {
            service = _notificationServices
                .First(x => x is EmailNotificationService);
        }
        else
        {
            service = _notificationServices
                .First(x => x is SmsNotificationService);
        }

        service.Send(message);
    }
}
```

Then:

```csharp
orderService.SendNotification("email", "Order placed");
```

uses:

```text
EmailNotificationService
```

while:

```csharp
orderService.SendNotification("sms", "Order shipped");
```

uses:

```text
SmsNotificationService
```

---

## ⭐ Better architecture: Factory

For a real application, I would avoid putting `if/else` and `is` checks inside `OrderService`.

Instead:

```text
OrderService
     |
     v
NotificationFactory
     |
     +----> EmailNotificationService
     |
     +----> SmsNotificationService
```

Then `OrderService` only knows about the factory:

```csharp
public class OrderService
{
    private readonly INotificationFactory _factory;

    public OrderService(INotificationFactory factory)
    {
        _factory = factory;
    }

    public void PlaceOrder(string notificationType)
    {
        var notification = _factory.GetService(notificationType);

        notification.Send("Order placed successfully.");
    }
}
```

### Interview answer

> **"In .NET 6, I can register multiple implementations of the same interface. If I need only one implementation, I can inject `INotificationService`. If I need to dynamically choose Email or SMS at runtime, I can inject `IEnumerable<INotificationService>` or, preferably, create a Factory that encapsulates the selection logic. The consuming class then depends on the abstraction rather than directly creating Email or SMS objects."**

This is a very common **DI + Factory Pattern** interview scenario.



---

### 2. Singleton service depends on Scoped service

**Question:**
What happens if a Singleton service depends on a Scoped service?

**Answer:**

> “This creates a **captive dependency** problem because the Singleton lives for the entire application lifetime, while the Scoped service should live only for a request. In development, .NET's scope validation can detect this. I would normally redesign the dependency or create an explicit scope when appropriate.”

**Rule to remember:**

```text
Singleton → Scoped ❌
Singleton → Transient ✅
Scoped    → Singleton ✅
Scoped    → Transient ✅
```

In ASP.NET Core, **a Singleton should not directly depend on a Scoped service**.

### What happens?

If you try:

```csharp
builder.Services.AddSingleton<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
```

and:

```csharp
public OrderService(IOrderRepository repository)
{
    // ...
}
```

ASP.NET Core's DI container will generally throw an exception when validating/resolving the service:

> **Cannot consume scoped service 'IOrderRepository' from singleton 'IOrderService'.**

### Why?

Because:

```text
Singleton → lives for entire application
Scoped    → lives for one request
```

The Singleton would potentially hold a reference to a Scoped object **after that scope/request has ended**, which is unsafe and breaks the intended lifetime.

### Interview answer 🎯

> **"A Singleton cannot directly depend on a Scoped service because their lifetimes are incompatible. The Singleton lives for the application's lifetime, while the Scoped service is created per request. ASP.NET Core's DI container detects this captive dependency and throws an exception."**

**Rule to remember:**

```text
Singleton → Singleton ✅
Singleton → Transient ✅ (with caution)
Singleton → Scoped ❌

Scoped → Singleton ✅
Scoped → Scoped ✅
Scoped → Transient ✅
```
---

### 3. DbContext lifetime

**Question:**
You are developing an ASP.NET Core Web API using Entity Framework Core. What lifetime would you choose for `DbContext` and why?

**Answer:**

> “I would normally use **Scoped** lifetime for `DbContext`. A scoped `DbContext` gives us one context per HTTP request, which fits the unit-of-work pattern and avoids sharing the same context across concurrent requests.”

```csharp
builder.Services.AddDbContext<AppDbContext>();
```

`AddDbContext()` registers it as **Scoped by default**.

---

### 4. Service needs a different dependency based on configuration

**Question:**
Your application can use either Azure Blob Storage or AWS S3 depending on configuration. How would you implement this using DI?

**Answer:**

> “I would create an abstraction such as `IFileStorageService` and have separate implementations for Azure Blob and AWS S3. I would register the implementations through DI and select the appropriate implementation based on configuration. This keeps the business logic independent of the storage provider.”

```text
IFileStorageService
       |
       +-- AzureBlobStorageService
       |
       +-- S3StorageService
```

This is a good example of **Dependency Inversion + DI**.

---

### 5. Transient service maintains state unexpectedly

**Question:**
You registered a service as `Transient`, but you notice that its state is not maintained between calls. Why?

**Answer:**

> “That's expected. A Transient service creates a **new instance every time it is requested**. If I need the same instance throughout an HTTP request, I would use Scoped. If I need one instance for the application's lifetime, I would use Singleton.”

```text
Transient → New instance every time
Scoped    → One instance per request/scope
Singleton → One instance for application lifetime
```

### ⭐ Interview shortcut

If the interviewer asks **"How do you decide the DI lifetime?"**, remember:

> **Transient = lightweight/stateless**
> **Scoped = request-specific / DbContext**
> **Singleton = shared, thread-safe, application-wide state**

### 6. What happens if a Singleton service depends on a Scoped service?  

In ASP.NET Core, **a Singleton should not directly depend on a Scoped service**.

### What happens?

If you try:

```csharp
builder.Services.AddSingleton<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
```

and:

```csharp
public OrderService(IOrderRepository repository)
{
    // ...
}
```

ASP.NET Core's DI container will generally throw an exception when validating/resolving the service:

> **Cannot consume scoped service 'IOrderRepository' from singleton 'IOrderService'.**

### Why?

Because:

```text
Singleton → lives for entire application
Scoped    → lives for one request
```

The Singleton would potentially hold a reference to a Scoped object **after that scope/request has ended**, which is unsafe and breaks the intended lifetime.

### Interview answer 🎯

> **"A Singleton cannot directly depend on a Scoped service because their lifetimes are incompatible. The Singleton lives for the application's lifetime, while the Scoped service is created per request. ASP.NET Core's DI container detects this captive dependency and throws an exception."**

**Rule to remember:**

```text
Singleton → Singleton ✅
Singleton → Transient ✅ (with caution)
Singleton → Scoped ❌

Scoped → Singleton ✅
Scoped → Scoped ✅
Scoped → Transient ✅
```
> Note: In a plain Console App, the DI container doesn't automatically validate everything unless you enable validation. We'll enable it so you can clearly see the error.

### 1. Create the project

```bash
dotnet new console -n DependencyLifetimeDemo
cd DependencyLifetimeDemo
dotnet add package Microsoft.Extensions.DependencyInjection
```

### 2. Replace `Program.cs`

```csharp
using Microsoft.Extensions.DependencyInjection;

// Create DI container
var services = new ServiceCollection();

// Register services
services.AddSingleton<IOrderService, OrderService>();
services.AddScoped<IOrderRepository, OrderRepository>();

// Enable DI validation
var serviceProvider = services.BuildServiceProvider(
    new ServiceProviderOptions
    {
        ValidateScopes = true,
        ValidateOnBuild = true
    });

Console.WriteLine("Application started.");
```

### 3. Add the interfaces/classes

You can put these in the same `Program.cs` for testing:

```csharp
public interface IOrderService
{
    void ProcessOrder();
}

public interface IOrderRepository
{
    void Save();
}

public class OrderService : IOrderService
{
    private readonly IOrderRepository _repository;

    public OrderService(IOrderRepository repository)
    {
        _repository = repository;
    }

    public void ProcessOrder()
    {
        _repository.Save();
    }
}

public class OrderRepository : IOrderRepository
{
    public void Save()
    {
        Console.WriteLine("Order saved.");
    }
}
```

### What happens when you run it?

Run:

```bash
dotnet run
```

You should get an exception similar to:

```text
System.AggregateException:
Some services are not able to be constructed

Cannot consume scoped service
'IOrderRepository'
from singleton
'IOrderService'.
```

### Why?

Your dependency chain is:

```text
IOrderService
    ↓
Singleton
    ↓
IOrderRepository
    ↓
Scoped ❌
```

The problem is that the Singleton lives for the **entire application lifetime**, while the Scoped service is supposed to live only within a **scope**.

---

## How to fix it

Make `OrderService` scoped:

```csharp
services.AddScoped<IOrderService, OrderService>();
services.AddScoped<IOrderRepository, OrderRepository>();
```

Now:

```text
IOrderService
    ↓
Scoped
    ↓
IOrderRepository
    ↓
Scoped
```

This is valid. ✅

### Interview point

Remember this rule:

```text
Singleton → Scoped       ❌
Singleton → Transient    ⚠️
Singleton → Singleton    ✅

Scoped → Singleton       ✅
Scoped → Scoped          ✅
Scoped → Transient       ✅
```

The important interview term here is **"captive dependency"** — when a longer-lived service holds a shorter-lived dependency.
